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
        public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
        // Use "*/10 * * * * *" for 10-second testing
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // 1. Get Connection String from Environment (Azure) or local settings
            string connString = (Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection")
                    ?? Environment.GetEnvironmentVariable("DefaultConnection"))
                    ?? string.Empty;

            var optionsBuilder = new DbContextOptionsBuilder<ApiDbContext>();

            // 2. Determine Environment and Configure DB
            if (!string.IsNullOrEmpty(connString) && (connString.Contains("database.windows.net") || connString.Contains("Server=")))
            {
                // CLOUD MODE: Use Azure SQL
                _logger.LogInformation("Using Azure SQL Database.");
                optionsBuilder.UseSqlServer(connString);
            }
            else
            {
                // LOCAL MODE: Use SQLite
                string executionDir = AppContext.BaseDirectory;
                // Adjust path to find the .db file relative to your function execution folder
                string dbPath = Path.GetFullPath(Path.Combine(executionDir, "..\\..\\..\\..\\WebApplication1\\tinyurl.db"));

                _logger.LogInformation($"Using Local SQLite at: {dbPath}");

                if (!File.Exists(dbPath))
                {
                    _logger.LogError($"SQLite FILE NOT FOUND at: {dbPath}");
                    return;
                }
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }

            // 3. Execute Cleanup Logic
            using (var db = new ApiDbContext(optionsBuilder.Options))
            {
                try
                {
                    // Efficiency Tip: Use ExecuteDeleteAsync for better performance in .NET 8
                    int count = await db.Urls.ExecuteDeleteAsync();
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