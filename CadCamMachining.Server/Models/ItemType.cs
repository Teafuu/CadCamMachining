using CadCamMachining.Server.Models.Properties;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models;

public class ItemType
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<ItemProperty>? Properties { get; set; } = new List<ItemProperty>();

    // Navigation properties for connections
    public ICollection<ItemTypeConnection>? ParentConnections { get; set; } = new List<ItemTypeConnection>();
    public ICollection<ItemTypeConnection>? ChildConnections { get; set; } = new List<ItemTypeConnection>();
}