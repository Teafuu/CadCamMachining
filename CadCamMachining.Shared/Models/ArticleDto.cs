using System;

namespace CadCamMachining.Shared.Models;

public class ArticleDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public PartDto PartDto { get; set; }

    public MaterialDto MaterialDto { get; set; }
    
    public OrderDto OrderDto { get; set; }
    
    public double Price { get; set; }
    
    public int Quantity { get; set; }
    
    public ArticleDto Status { get; set; }
    
    public DateTime LastUpdated { get; set; }
}