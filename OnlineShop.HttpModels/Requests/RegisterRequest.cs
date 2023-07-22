﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.HttpModel.Requests;

public class RegisterRequest
{
    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}