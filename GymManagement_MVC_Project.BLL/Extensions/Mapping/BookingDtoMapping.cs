using GymManagement_MVC_Project.BLL.DTOs.Booking;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.BLL.Extensions.Mapping;

public static class BookingDtoMapping
{
    public static BookingIndexDto GetIndexDto(this BookingIndexDtoQS indexDtoQS)
    {
        return new BookingIndexDto
        {
            Id = indexDtoQS.Id,
            CategoryName = indexDtoQS.CategoryName,
            Status = SessionDtoMapping.GetSessionStatus(indexDtoQS.StartDate, indexDtoQS.EndDate),
            Description = indexDtoQS.Description,
            TrainerName = indexDtoQS.TrainerName,
            StartDate = indexDtoQS.StartDate,
            EndDate = indexDtoQS.EndDate,
            Capacity = indexDtoQS.Capacity,
            BookedCount = indexDtoQS.BookedCount,
            IsDeleted = indexDtoQS.IsDeleted
        };
    }

    public static BookingViewMembersDto GetBookingDto(this BookingViewMembersDtoQS dtoQS)
    {
        return new BookingViewMembersDto
        {
            SessionDetails = dtoQS.SessionDetails.GetMainSessionDetailsDto(),
            SessionMembers = [.. dtoQS.SessionMembers.Select(m => m.GetBookingMember())]
        };
    }

    //public static BookingCreateDto GetCreateDto(this BookingCreateDtoQS dtoQS)
    //{
    //    return new BookingCreateDto
    //    {
    //        SessionDetails = dtoQS.SessionDetails.GetMainSessionDetailsDto(),
    //        Members = [..dtoQS.Members.Select(m => new DTOs.Booking.MemberLookupItem
    //        {
    //            Id = m.Id,
    //            Name = m.Name
    //        })]
    //    };
    //}

    public static BookingMainSessionDetailsDto GetMainSessionDetailsDto(this BookingMainSessionDetailsDtoQS dtoQS)
    {
        return new BookingMainSessionDetailsDto
        {
            SessionId = dtoQS.SessionId,
            CategoryName = dtoQS.CategoryName,
            TrainerName = dtoQS.TrainerName,
            Capacity = dtoQS.Capacity,
            StartDate = dtoQS.StartDate,
            EndDate = dtoQS.EndDate
        };
    }

    public static BookingSessionMemberDto GetBookingMember(this BookingSessionMemberDtoQS dtoQS)
    {
        return new BookingSessionMemberDto
        {
            Id = dtoQS.Id,
            MemberName = dtoQS.MemberName,
            BookingDate = dtoQS.BookingDate,
            IsAttended = dtoQS.IsAttended
        };
    }

    public static Booking GetBooking(this BookingCreateDto createDto, IDateTimeProvider clock)
    {
        return new Booking
        {
            MemberId = createDto.MemberId,
            SessionId = createDto.SessionId,
            BookingDate = clock.UtcNow,
            IsAttended = false
        };
    }
}
