using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Plan;

public class PlanEditViewModel
{
    [Required(ErrorMessage = "Name Is Required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
    public string Name { get; set; } = default!;


    [Required(ErrorMessage = "Description Is Required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Duration days Is Required")]
    [Range(30, 365, ErrorMessage = "Duration days must be start from 30 to 365 ")]
    public int DurationDays { get; set; }

    [Required(ErrorMessage = "Price Is Required")]
    [Range(100, 9000, ErrorMessage = "Price must be start from 100")]
    public decimal Price { get; set; }
}
