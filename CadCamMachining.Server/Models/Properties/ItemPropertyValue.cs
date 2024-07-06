using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CadCamMachining.Server.Models.Properties
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(StringPropertyValue), typeof(EnumPropertyValue), typeof(BoolPropertyValue), typeof(DateTimePropertyValue))]
    public abstract class ItemPropertyValue
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string ItemPropertyId { get; set; } = string.Empty;
    }

    public class StringPropertyValue : ItemPropertyValue
    {
        public string Value { get; set; }
    }

    public class EnumPropertyValue : ItemPropertyValue
    {
        public string Value { get; set; } // Store the selected option
    }

    public class BoolPropertyValue : ItemPropertyValue
    {
        public bool Value { get; set; }
    }

    public class DateTimePropertyValue : ItemPropertyValue
    {
        public DateTime Value { get; set; }
    }
}
