using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models;

public class UserRegisterModel
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    [Required]
    [MinLength(6)]
    public string password { get; set; }
    [Required]
    [MinLength(1)]
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+")]
    public string email { get; set; }
    public string? address { get; set; }
    public DateTime? birthDate { get; set; }
    [Required]
    public Gender gender { get; set; }
    [Phone]
    public string? phoneNumber { get; set; }
}