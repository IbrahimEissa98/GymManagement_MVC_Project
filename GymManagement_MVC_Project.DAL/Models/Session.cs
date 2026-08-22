using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.DAL.Models;

public class Session : BaseEntity
{
    [Range(1, 25)]
    public int Capacity { get; set; }
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;

    public ICollection<Booking> Bookings { get; set; } = [];
}