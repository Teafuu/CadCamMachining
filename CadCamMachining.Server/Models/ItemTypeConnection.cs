using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models
{
    public class ItemTypeConnection
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string ParentItemTypeId { get; set; } = string.Empty;

        public string ChildItemTypeId { get; set; } = string.Empty;
    }
}
