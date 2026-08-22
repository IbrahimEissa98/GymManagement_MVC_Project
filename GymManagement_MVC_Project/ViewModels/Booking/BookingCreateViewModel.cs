using GymManagement_MVC_Project.BLL.DTOs.Booking;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Booking;

public class BookingCreateViewModel
{
    public BookingMainSessionDetailsViewModel? SessionDetails { get; set; } = default!;

    [Required(ErrorMessage = "Member is required.")]
    [Range(1, int.MaxValue)]
    public int MemberId { get; set; }

    [Required(ErrorMessage = "Session is required.")]
    [Range(1, int.MaxValue)]
    public int SessionId { get; set; }

    public IReadOnlyList<MemberLookupItem> Members { get; set; } = [];
}
