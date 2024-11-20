using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SkillNet.Notifications.Entities
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id", Order = 1)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("title")]
        public string Title { get; set; } = default!;

        [BsonElement("message")] 
        public string Message { get; set; } = default!;

        [BsonElement("user-id")]
        public string UserId { get; set; } = default!;

        [BsonElement("timestamp")]
        public DateTime Date { get; set; }
    }
}
