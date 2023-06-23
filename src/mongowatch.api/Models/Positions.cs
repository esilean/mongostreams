using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace mongowatch.api.Models
{
    public class Position
    {
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
    }

        public class PositionExpiration
    {
        public ObjectId Id { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}