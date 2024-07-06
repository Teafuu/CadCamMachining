using System;

namespace CadCamMachining.Shared.Models
{
    public class ItemTypeConnectionDto
    {
        public string Id { get; set; } = string.Empty;

        public string ParentItemTypeId { get; set; } = string.Empty;

        public string ChildItemTypeId { get; set; } = string.Empty;
    }
}
