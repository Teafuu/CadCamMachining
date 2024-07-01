namespace CadCamMachining.Server.Models;

public class Material
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime LastUpdated { get; set; }
}