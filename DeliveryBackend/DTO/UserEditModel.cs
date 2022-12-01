﻿using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.DTO;

public class UserEditModel
{
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    [Required]
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
}