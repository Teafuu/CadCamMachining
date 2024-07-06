using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models.Layouts
{
    public class Layout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public ICollection<Component> Components { get; set; }
    }
}
