using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompanyManagementSystem.Data
{
    public class DbService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbService> _logger;
        private readonly string _connectionString;

        public DbService(IConfiguration configuration, ILogger<DbService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public SqlConnection GetConnection()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                _logger.LogInformation("SQL connection created successfully.");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create SQL connection.");
                throw;
            }
        }

        public T DbConnection<T>(Func<SqlConnection, T> func)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    _logger.LogInformation("SQL connection opened successfully.");
                    return func(conn);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while using the SQL connection.");
                throw;
            }
        }
    }
}
