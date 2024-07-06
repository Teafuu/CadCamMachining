using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models.Layouts
{
    public class Component
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string DropzoneIdentifier { get; set; } = string.Empty;

        public string ItemTypeId { get; set; } = string.Empty;

        public string ComponentType { get; set; }
    }
}
