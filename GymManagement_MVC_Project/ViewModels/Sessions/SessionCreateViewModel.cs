using GymManagement_MVC_Project.BLL.DTOs.Session;
using GymManagement_MVC_Project.DAL.Queries.DTOs;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Sessions;

public class SessionCreateViewModel
{
    [Required(ErrorMessage = "Description Is Required")]
    [StringLength(300, MinimumLength = 10)]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Capacity Is Required")]
    [Range(1, 25)]
    public int Capacity { get; set; } = 25;

    [Required(ErrorMessage = "Start Date Is Required")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End Date Is Required")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Trainer Is Required")]
    [Range(1, int.MaxValue)]
    public int TrainerId { get; set; }

    [Required(ErrorMessage = "Category Is Required")]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public IReadOnlyList<CategoryLookupItem> Categories { get; set; } = [];
    public IReadOnlyList<TrainerLookupItem> Trainers { get; set; } = [];
}
