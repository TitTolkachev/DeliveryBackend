using System.ComponentModel.DataAnnotations;

namespace DeliveryBackend.Data.Models.DTO;

public class LoginCredentials
{
    [Required]
    [MinLength(1)]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    [MinLength(1)]
    public string password { get; set; }
}