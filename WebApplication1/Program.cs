using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApiDbContext>(options =>options.UseSqlite("Data Source=tinyurl.db"));
var app = builder.Build();



// --- UTILITY LOGIC ---

string GenerateShortCode() => Guid.NewGuid().ToString("n").Substring(0, 6);

// --- API ROUTES ---

// 1. POST: Create a Short URL

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

    return Results.Redirect(entry.LongUrl);
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

// DTO (Data Transfer Object) for the POST request
public record TinyUrlRequest(string LongUrl, bool IsPrivate);
