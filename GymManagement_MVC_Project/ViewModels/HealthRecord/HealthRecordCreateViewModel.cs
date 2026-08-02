using GymManagement_MVC_Project.DAL.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.HealthRecord;

public class HealthRecordCreateViewModel
{
    [Range(0.1, 300, ErrorMessage = "Height must be greater than 0")]
    public int Height { get; set; }

    [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0")]
    public int Weight { get; set; }

    [Required(ErrorMessage = "Blood Type Is Required")]
    [StringLength(3, ErrorMessage = "Blood type must be 3 characters or less")]
    [EnumDataType(typeof(BloodTypes), ErrorMessage = "Invalid blood type")]
    public string BloodType { get; set; } = default!;
    public string? Note { get; set; } = default!;
}
