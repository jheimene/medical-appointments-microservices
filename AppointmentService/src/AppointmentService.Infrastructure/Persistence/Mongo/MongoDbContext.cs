
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace OrderService.Infrastructure.Persistence.Mongo
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        //private readonly IMongoClient _client;

        //public IMongoClient Client { get => _client; }

        public MongoDbContext(
            IMongoDatabase database//,
            //IMongoClient mongoClient
            /* IOptions<MongoSettings> settings */)
        {
            //if (settings.Value is null)
            //{
            //    throw new ArgumentNullException(nameof(settings));
            //}

            //_client = new MongoClient(settings.Value.ConnectionString);
            //_database = _client.GetDatabase(settings.Value.DatabaseName);

            //_client = mongoClient;
            _database = database;

            var conventionPack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
            };
            ConventionRegistry.Register("EnumAsString", conventionPack, t => true);
        }

        public IMongoCollection<OrderDocument> Orders => _database.GetCollection<OrderDocument>("Orders");

    }
}
