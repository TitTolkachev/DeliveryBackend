using System.ComponentModel.DataAnnotations;

namespace DeliveryBackend.Data.Models.DTO;

public class PageInfoModel
{
    [Required]
    public int size { get; set; }
    [Required]
    public int count { get; set; }
    [Required]
    public int current { get; set; }
}