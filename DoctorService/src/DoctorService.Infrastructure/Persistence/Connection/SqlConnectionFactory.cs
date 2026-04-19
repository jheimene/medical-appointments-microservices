using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProductService.Infrastructure.Persistence.Factories;
using System.Data;

namespace ProductService.Infrastructure.Persistence.Connection
{
    public sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
