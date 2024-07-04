using System;

namespace CadCamMachining.Shared.Models;

public class Part
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public DateTime LastUpdated { get; set; }
}