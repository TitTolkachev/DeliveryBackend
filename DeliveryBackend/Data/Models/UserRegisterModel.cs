using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models;

public class UserRegisterModel
{
    public string fullName { get; set; }
    public string password { get; set; }
    public string email { get; set; }
    public string? address { get; set; }
    public DateTime? birthDate { get; set; }
    public Gender gender { get; set; }
    public string? phoneNumber { get; set; }
}