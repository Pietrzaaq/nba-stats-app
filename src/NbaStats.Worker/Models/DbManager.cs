using Microsoft.Extensions.Configuration;

namespace NbaStats.Worker.Models
{
    public class DbManager
    {
        private readonly IConfiguration _configuration;

        public DbManager()
        {
            
        }
        
        public string GetConnectionString()
        {
            return "Server=localhost;Database=NbaDatabase;Trusted_Connection=True";
        }
    }
}