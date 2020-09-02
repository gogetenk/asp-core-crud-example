using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dal.Impl.Entities
{
    public class MongoEntityBase
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
