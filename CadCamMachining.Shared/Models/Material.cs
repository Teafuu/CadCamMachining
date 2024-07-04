using System;

namespace CadCamMachining.Shared.Models;

public class Material
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime LastUpdated { get; set; }
}