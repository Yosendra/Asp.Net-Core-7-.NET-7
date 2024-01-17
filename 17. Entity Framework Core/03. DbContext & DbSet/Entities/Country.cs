﻿using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Domain Model for Country
/// </summary>
public class Country
{
    [Key] // notice this
    public Guid Id { get; set; }
    public string? Name { get; set; }
}