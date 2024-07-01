namespace CadCamMachining.Server.Models;

public class Contact
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string Surname { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Customer Customer { get; set; }
    
    public DateTime LastUpdated { get; set; }
}