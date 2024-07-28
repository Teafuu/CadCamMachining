using System;

namespace CadCamMachining.Shared.Models
{
    public class ComponentDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string DropzoneIdentifier { get; set; } = string.Empty;

        public string ItemTypeId { get; set; } = string.Empty;

        public string ComponentType { get; set; }

        public int Index { get; set; } = 0;
    }
}
