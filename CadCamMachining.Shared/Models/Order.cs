using System;
using System.Collections.Generic;

namespace CadCamMachining.Shared.Models;

public class Order
{
    public Guid Id { get; set; }

    public string OrderNo { get; set; }

    public string Name { get; set; }

    public ICollection<Article>? Articles { get; set; }

    public OrderStatus Status { get; set; }

    public Customer? Customer { get; set; }

    public Contact Contact { get; set; }

    public UserInfo InCharge { get; set; }

    public DateTime LastUpdated { get; set; }
}