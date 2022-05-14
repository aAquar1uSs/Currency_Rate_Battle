﻿using System.ComponentModel.DataAnnotations;

namespace CurrencyRateBattle_Server.Models;

public sealed class Account
{
    [Key]
    public int Id { get; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}