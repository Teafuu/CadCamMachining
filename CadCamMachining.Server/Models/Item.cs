using CadCamMachining.Server.Models.Properties;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using CadCamMachining.Shared.Models;

namespace CadCamMachining.Server.Models
{
    public class Item
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ItemTypeId { get; set; } = string.Empty;  

        public List<ItemPropertyValue>? PropertyValues { get; set; } = new List<ItemPropertyValue>();

        public List<ItemConnection>? ParentConnections { get; set; } = new();

        public List<ItemConnection>? ChildConnections { get; set; } = new();
    }

}
