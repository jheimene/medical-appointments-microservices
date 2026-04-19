
using Microsoft.Data.SqlClient;
using ProductService.Infrastructure.Persistence.Factories;
using System.Data;

namespace ProductService.Infrastructure.Persistence.Repositories
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            return connection;
        }
    }
}
