using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using mongowatch.api.Models;

namespace mongowatch.api.Data
{
    public class MongoWatcher
    {
        private readonly IMongoCollection<PositionExpiration> _positionsExpirationCollection;

        public MongoWatcher()
        {
            var client = new MongoClient("mongodb://localhost:27021/?compressors=disabled&gssapiServiceName=mongodb");
            var db = client.GetDatabase("mongowatch");

            _positionsExpirationCollection = db.GetCollection<PositionExpiration>("PositionsExpiration");
        }

        public void Start()
        {
            var options = new ChangeStreamOptions { };
            var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<PositionExpiration>>()
                                    .Match("{ operationType: { $in: [ 'insert', 'delete' ] } }");

            var cursor = _positionsExpirationCollection.Watch<ChangeStreamDocument<PositionExpiration>>(pipeline, options);

            var enumerator = cursor.ToEnumerable().GetEnumerator();

            Console.WriteLine("Running start...");

            while(enumerator.MoveNext())
            {
                ChangeStreamDocument<PositionExpiration> doc = enumerator.Current;
                // Do something here with your document
                Console.WriteLine($"{doc.DocumentKey} - {DateTime.Now}"); 
                Console.WriteLine($"{doc.OperationType} - {DateTime.Now}"); 
            }

            Console.WriteLine("Finishing start..."); 
        }
    }
}