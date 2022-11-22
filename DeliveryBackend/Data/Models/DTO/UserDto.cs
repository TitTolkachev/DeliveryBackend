using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class UserDto
{
    public Guid id { get; set; }
    public string fullName { get; set; }
    public DateTime? birthDate { get; set; }
    public Gender gender { get; set; }
    public string? address { get; set; }
    public string? email { get; set; }
    public string? phoneNumber { get; set; }
}