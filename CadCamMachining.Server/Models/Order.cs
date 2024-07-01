namespace CadCamMachining.Server.Models;

public class Order
{
    public Guid Id { get; set; }
    
    public string OrderNo { get; set; }
    
    public string Name { get; set; }

    public ICollection<Article>? Articles { get; set; }
    
    public Customer? Customer { get; set; }
    
    public string Status { get; set; }
    
    public DateTime LastUpdated { get; set; }
}