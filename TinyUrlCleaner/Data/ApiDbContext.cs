using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace TinyUrlCleaner.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
        public DbSet<TinyUrl> Urls { get; set; }
    }
}
