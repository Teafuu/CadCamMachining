using System;
using System.Text.Json.Serialization;

namespace CadCamMachining.Shared.Models
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(StringPropertyValueDto), "StringPropertyValueDto")]
    [JsonDerivedType(typeof(EnumPropertyValueDto), "EnumPropertyValueDto")]
    [JsonDerivedType(typeof(BoolPropertyValueDto), "BoolPropertyValueDto")]
    [JsonDerivedType(typeof(DateTimePropertyValueDto), "DateTimePropertyValueDto")]
    public abstract class ItemPropertyValueDto
    {
        public string Id { get; set; } = string.Empty;

        public string ItemId { get; set; }

        public string ItemPropertyId { get; set; } = string.Empty;

    }

    [JsonDerivedType(typeof(StringPropertyValueDto), "StringPropertyValueValueDto")]
    public class StringPropertyValueDto : ItemPropertyValueDto
    {
        public string Value { get; set; } = string.Empty;
        public override string ToString()
        {
            return Value;
        }
    }

    [JsonDerivedType(typeof(EnumPropertyValueDto), "EnumPropertyValueDto")]
    public class EnumPropertyValueDto : ItemPropertyValueDto
    {
        public string Value { get; set; } = string.Empty;

        public override string ToString()
        {
            return Value;
        }
    }

    [JsonDerivedType(typeof(BoolPropertyValueDto), "BoolPropertyValueDto")]
    public class BoolPropertyValueDto : ItemPropertyValueDto
    {
        public bool Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [JsonDerivedType(typeof(DateTimePropertyValueDto), "DateTimePropertyValueDto")]
    public class DateTimePropertyValueDto : ItemPropertyValueDto
    {
        public DateTime? Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
