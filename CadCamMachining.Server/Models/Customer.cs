namespace CadCamMachining.Server.Models;

public class Customer
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public ICollection<Contact> Contacts { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
    public DateTime LastUpdated { get; set; }
}