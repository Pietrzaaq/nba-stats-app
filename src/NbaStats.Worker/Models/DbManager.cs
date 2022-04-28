using Microsoft.Extensions.Configuration;

namespace NbaStats.Worker.Models
{
    public class DbManager
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DbManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = GetConnectionString();
        }
        
        public string GetConnectionString()
        {
            return _configuration[$"database:{nameof(DatabaseOptions.ConnectionString)}"];
        }
    }
}