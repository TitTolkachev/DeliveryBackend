using System.ComponentModel.DataAnnotations;

namespace DeliveryBackend.Data.Models;

public class LoginCredentials
{
    [Required]
    [MinLength(1)]
    [RegularExpression(@"[a-zA-Z]+\w*@[a-zA-Z]+\.[a-zA-Z]+")]
    public string email { get; set; }
    [Required]
    [MinLength(1)]
    public string password { get; set; }
}