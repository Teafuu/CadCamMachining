using System.Collections.Generic;

namespace CadCamMachining.Shared.Models
{
    public class LayoutDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public List<ComponentDto> Components { get; set; } = new List<ComponentDto>();
    }
}
