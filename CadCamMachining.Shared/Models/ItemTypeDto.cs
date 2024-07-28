using System.Collections.Generic;

namespace CadCamMachining.Shared.Models;

public class ItemTypeDto
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public List<ItemPropertyDto>? Properties { get; set; } = new List<ItemPropertyDto>();

    public List<ItemTypeConnectionDto> ParentConnections { get; set; } = new List<ItemTypeConnectionDto>();

    public List<ItemTypeConnectionDto> ChildConnections { get; set; } = new List<ItemTypeConnectionDto>();
}