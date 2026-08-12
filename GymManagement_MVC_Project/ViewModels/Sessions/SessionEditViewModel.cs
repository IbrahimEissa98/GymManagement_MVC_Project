using GymManagement_MVC_Project.DAL.Queries.DTOs;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Sessions;

public class SessionEditViewModel
{
    [Required(ErrorMessage = "Description Is Required")]
    [StringLength(300, MinimumLength = 10)]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Start Date Is Required")]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End Date Is Required")]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Trainer Is Required")]
    [Range(1, int.MaxValue)]
    public int TrainerId { get; set; }
    public int CategoryId { get; init; }

    public IReadOnlyList<TrainerLookupItem> Trainers { get; set; } = [];
}
