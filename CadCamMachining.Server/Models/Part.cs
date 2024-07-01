namespace CadCamMachining.Server.Models;

public class Part
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Location { get; set; }
    
    public DateTime LastUpdated { get; set; }
}