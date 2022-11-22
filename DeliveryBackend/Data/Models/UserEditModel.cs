using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models;

public class UserEditModel
{
    public string fullName { get; set; }
    public DateTime? birthDate { get; set; }
    public Gender gender { get; set; }
    public string? address { get; set; }
    public string? phoneNumber { get; set; }
}