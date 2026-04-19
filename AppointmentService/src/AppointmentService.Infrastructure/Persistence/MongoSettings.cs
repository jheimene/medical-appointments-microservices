
namespace OrderService.Infrastructure.Persistence
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
