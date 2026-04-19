using System.Data;

namespace ProductService.Infrastructure.Persistence.Factories
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
