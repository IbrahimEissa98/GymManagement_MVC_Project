namespace GymManagement_MVC_Project.DAL.Queries.DTOs;

public class BookingCreateDtoQS
{
    public BookingMainSessionDetailsDtoQS SessionDetails { get; set; } = default!;

    public IReadOnlyList<MemberLookupItem> Members { get; set; } = [];
}
