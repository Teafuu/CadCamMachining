﻿using System;

namespace CadCamMachining.Shared.Models;

public class Article
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public Part Part { get; set; }

    public Material Material { get; set; }
    
    public Order Order { get; set; }
    
    public double Price { get; set; }
    
    public int Quantity { get; set; }
    
    public Article Status { get; set; }
    
    public DateTime LastUpdated { get; set; }
}