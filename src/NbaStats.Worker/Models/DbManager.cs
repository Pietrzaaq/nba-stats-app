using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using NbaStats.Domain.Entities;



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
        
        private string GetConnectionString()
        {
            return _configuration[$"database:{nameof(DatabaseOptions.ConnectionString)}"];
        }

        public void GetGames()
        {
            string sql = "SELECT * FROM StatsManagement.Games";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var games = connection.Query<Game>(sql);
            }
        }
    }
}