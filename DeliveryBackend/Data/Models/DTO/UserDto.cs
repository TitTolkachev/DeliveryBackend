using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class UserDto
{
    public Guid id { get; set; }
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    public DateTime? birthDate { get; set; }
    [Required]
    public Gender gender { get; set; }
    public string? address { get; set; }
    [MinLength(1)]
    [EmailAddress]
    public string? email { get; set; }
    [Phone]
    public string? phoneNumber { get; set; }
}