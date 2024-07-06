using System.Collections.Generic;

namespace CadCamMachining.Shared.Models
{
    public class ItemDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ItemTypeId { get; set; } = string.Empty;

        public List<ItemPropertyValueDto>? PropertyValues { get; set; } = new();

        public List<ItemConnectionDto>? ParentConnections { get; set; } = new();

        public List<ItemConnectionDto>? ChildConnections { get; set; } = new();
    }
}
