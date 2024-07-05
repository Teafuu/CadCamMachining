using System;

namespace CadCamMachining.Shared.Models;

public class ContactDto
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string Surname { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public CustomerDto CustomerDto { get; set; }
    
    public DateTime LastUpdated { get; set; }
}