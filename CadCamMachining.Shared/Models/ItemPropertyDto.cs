using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CadCamMachining.Shared.Models
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(StringPropertyDto), "StringPropertyDto")]
    [JsonDerivedType(typeof(EnumPropertyDto), "EnumPropertyDto")]
    [JsonDerivedType(typeof(BoolPropertyDto), "BoolPropertyDto")]
    [JsonDerivedType(typeof(DateTimePropertyDto), "DateTimePropertyDto")]
    public abstract class ItemPropertyDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public PropertyTypeDto PropertyType { get; set; }
    }

    [JsonDerivedType(typeof(StringPropertyDto), "StringPropertyDto")]
    public class StringPropertyDto : ItemPropertyDto
    {
        public string DefaultValue { get; set; } = string.Empty;
    }

    [JsonDerivedType(typeof(EnumPropertyDto), "EnumPropertyDto")]
    public class EnumPropertyDto : ItemPropertyDto
    {
        public List<string> Options { get; set; } = new List<string>();
    }

    [JsonDerivedType(typeof(BoolPropertyDto), "BoolPropertyDto")]
    public class BoolPropertyDto : ItemPropertyDto
    {
        public bool DefaultValue { get; set; } = false;
    }

    [JsonDerivedType(typeof(DateTimePropertyDto), "DateTimePropertyDto")]
    public class DateTimePropertyDto : ItemPropertyDto
    {
        public DateTime DefaultValue { get; set; } = DateTime.MinValue;
    }
}
