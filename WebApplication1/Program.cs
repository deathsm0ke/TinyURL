using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddDbContext<ApiDbContext>(options =>options.UseSqlite("Data Source=tinyurl.db"));
var app = builder.Build();


app.UseCors("AllowAngular");

// --- UTILITY LOGIC ---

string GenerateShortCode() => Guid.NewGuid().ToString("n").Substring(0, 6);

// --- API ROUTES ---

// POST: Create a Short URL

app.MapPost("/api/url", async (TinyUrlRequest request, ApiDbContext db) =>
{
    if (string.IsNullOrEmpty(request.LongUrl))
        return Results.BadRequest("URL is required.");

    var newEntry = new TinyUrl
    {
        LongUrl = request.LongUrl,
        ShortCode = GenerateShortCode(),
        IsPrivate = request.IsPrivate,
        ClickCount = 0
    };

    db.Urls.Add(newEntry);
    await db.SaveChangesAsync();

    return Results.Ok(newEntry);
});

// GET: Public URLs List

app.MapGet("/api/public", async (ApiDbContext db) =>
{
    var publicUrls = await db.Urls
        .Where(u => !u.IsPrivate)
        .OrderByDescending(u => u.CreatedAt)
        .ToListAsync();

    return Results.Ok(publicUrls);
});

// GET: Redirection Logic

app.MapGet("/{code}", async (string code, ApiDbContext db) =>
{
    var entry = await db.Urls.FirstOrDefaultAsync(u => u.ShortCode == code);
    if (entry == null) return Results.NotFound();

    entry.ClickCount++;
    await db.SaveChangesAsync();

    // Add protocol, otherwise the browser stays on localhost
    var destination = entry.LongUrl.StartsWith("http")
                      ? entry.LongUrl
                      : $"https://{entry.LongUrl}";

    return Results.Redirect(destination);
});

// DELETE: Remove a Short URL

app.MapDelete("/api/delete/{code}", async (string code, ApiDbContext db) =>
{
    var entry = await db.Urls.FirstOrDefaultAsync(u => u.ShortCode == code);

    if (entry == null) return Results.NotFound();

    db.Urls.Remove(entry);
    await db.SaveChangesAsync();

    return Results.Ok(new { message = $"Deleted code: {code}" });
});

app.Run();


public record TinyUrlRequest(string LongUrl, bool IsPrivate);
