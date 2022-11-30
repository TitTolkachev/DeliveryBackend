using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class UserEditModel
{
    [Required]
    [MinLength(1)]
    public string fullName { get; set; }
    public DateTime? birthDate { get; set; }
    [Required]
    public Gender gender { get; set; }
    public string? address { get; set; }
    [Phone]
    public string? phoneNumber { get; set; }
}