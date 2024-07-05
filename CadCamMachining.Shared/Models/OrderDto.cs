using System;
using System.Collections.Generic;

namespace CadCamMachining.Shared.Models;

public class OrderDto
{
    public Guid Id { get; set; }

    public string OrderNo { get; set; }

    public string Name { get; set; }

    public ICollection<ArticleDto>? Articles { get; set; }

    public OrderStatusDto StatusDto { get; set; }

    public CustomerDto? Customer { get; set; }

    public ContactDto ContactDto { get; set; }

    public UserInfoDto InCharge { get; set; }

    public DateTime LastUpdated { get; set; }
}