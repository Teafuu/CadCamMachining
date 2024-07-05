using System;
using System.Collections.Generic;

namespace CadCamMachining.Shared.Models;

public class CustomerDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public ICollection<ContactDto> Contacts { get; set; }
    
    public ICollection<OrderDto> Orders { get; set; }
    
    public DateTime LastUpdated { get; set; }
}