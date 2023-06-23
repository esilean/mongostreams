using MongoDB.Driver;
using mongowatch.api.Models;

namespace mongowatch.api.Data
{
    public interface IMongoData
    {
        void AddPosition(Position position);

        IEnumerable<Position> ListAllPositions();

        IEnumerable<PositionExpiration> ListAllPositionsExpiration();
    }

    public class MongoData : IMongoData
    {
        private readonly ILogger<MongoData> _logger;
        private readonly IMongoCollection<Position> _positionsCollection;
        private readonly IMongoCollection<PositionExpiration> _positionsExpirationCollection;

        public MongoData(ILogger<MongoData> logger)
        {
            _logger = logger;

            var client = new MongoClient("mongodb://localhost:27021/?compressors=disabled&gssapiServiceName=mongodb");
            //"mongodb://127.0.0.1:27017/?compressors=disabled&gssapiServiceName=mongodb"
            var db = client.GetDatabase("mongowatch");

            _positionsCollection = db.GetCollection<Position>("Positions");
            _positionsExpirationCollection = db.GetCollection<PositionExpiration>("PositionsExpiration");


            _positionsExpirationCollection.Indexes.DropAll();

            var indexModel = new CreateIndexModel<PositionExpiration>(
            keys: Builders<PositionExpiration>.IndexKeys.Ascending("ExpiresAt"),
            options: new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.FromSeconds(0),
                Name = "ExpireAtIndex"
            });

            _positionsExpirationCollection.Indexes.CreateOne(indexModel);
        }

        public void AddPosition(Position position)
        {
            _positionsCollection.InsertOne(position);
            _positionsExpirationCollection.InsertOne(new PositionExpiration { Id = position.Id, ExpiresAt = DateTime.UtcNow.AddSeconds(10) });
        }

        public IEnumerable<Position> ListAllPositions()
        {
            return _positionsCollection.Find(x => true).ToList();
        }

        public IEnumerable<PositionExpiration> ListAllPositionsExpiration()
        {
            return _positionsExpirationCollection.Find(x => true).ToList();
        }
    }
}