using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models.Properties
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(StringProperty), typeof(EnumProperty), typeof(BoolProperty), typeof(DateTimeProperty))]
    public abstract class ItemProperty
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ItemTypeId { get; set; } = string.Empty;

        public PropertyType PropertyType { get; set; }
    }

    public class StringProperty : ItemProperty
    {
        public string DefaultValue { get; set; } = string.Empty;
    }

    public class EnumProperty : ItemProperty
    {
        public List<string> Options { get; set; } = new List<string>();
    }

    public class BoolProperty : ItemProperty
    {
        public bool DefaultValue { get; set; } = false;
    }

    public class DateTimeProperty : ItemProperty
    {
        public DateTime DefaultValue { get; set; } = DateTime.MinValue;
    }
}
