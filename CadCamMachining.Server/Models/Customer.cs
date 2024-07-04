namespace CadCamMachining.Server.Models;

public class Customer
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }

    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public DateTime LastUpdated { get; set; }
}