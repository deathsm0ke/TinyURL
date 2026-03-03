using System;
using Microsoft.Azure.Functions.Worker; 
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TinyUrlCleaner.Data;

namespace TinyUrlCleaner
{
    public class CleanupFunction
    {
        private readonly ILogger _logger;

        public CleanupFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CleanupFunction>();
        }


        [Function("CleanupOldUrls")]
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer) // for test 10 second Run([TimerTrigger("*/10 * * * * *")]
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Get the directory where the Function is running
            string executionDir = AppContext.BaseDirectory;


            
            string dbPath = Path.GetFullPath(Path.Combine(executionDir, "..\\..\\..\\..\\WebApplication1\\tinyurl.db"));

            _logger.LogInformation($"Looking for database at: {dbPath}");

            
            if (!File.Exists(dbPath))
            {
                _logger.LogError($"FILE NOT FOUND! Please check this path: {dbPath}");
                return;
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            using (var db = new ApiDbContext(optionsBuilder.Options))
            {
                try
                {
                    var allUrls = await db.Urls.ToListAsync();
                    int count = allUrls.Count;
                    db.Urls.RemoveRange(allUrls);
                    await db.SaveChangesAsync();
                    _logger.LogInformation($"SUCCESS: Deleted {count} URLs from the database.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Database error: {ex.Message}");
                }
            }
        }
    }

    
    public class TimerInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }
        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }
        public DateTime Next { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}