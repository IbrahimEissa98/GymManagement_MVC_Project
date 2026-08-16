using GymManagement_MVC_Project.BLL.DTOs.Booking;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.ViewModels.Booking;

namespace GymManagement_MVC_Project.PL.Extensions.Mapping;

public static class BookingVMMapping
{
    public static BookingIndexViewModel GetBookingIndexViewModel(
        this BookingIndexDto indexDto,
        IUserTimeZoneService zoneService)
    {
        return new BookingIndexViewModel
        {
            Id = indexDto.Id,
            CategoryName = indexDto.CategoryName,
            Status = indexDto.Status,
            Description = indexDto.Description,
            TrainerName = indexDto.TrainerName,
            Date = zoneService.ToUserTime(indexDto.StartDate).ToShortDateString(),
            TimeDisplay = $"{zoneService.ToUserTime(indexDto.StartDate):t} - {zoneService.ToUserTime(indexDto.EndDate):t}",
            Duration = (zoneService.ToUserTime(indexDto.EndDate) - zoneService.ToUserTime(indexDto.StartDate)),
            Capacity = $"{indexDto.BookedCount} / {indexDto.Capacity}",
            IsDeleted = indexDto.IsDeleted
        };
    }

    public static BookingViewMembersViewModel GetViewModel(
        this BookingViewMembersDto bookingDto,
        IUserTimeZoneService zoneService)
    {
        return new BookingViewMembersViewModel
        {
            SessionDetails = bookingDto.SessionDetails.GetMainSessionDetailsViewModel(),
            SessionMembers = [.. bookingDto.SessionMembers.Select(m => m.GetBookingMemberViewModel(zoneService))]
        };
    }

    //public static BookingCreateViewModel GetCreateViewModel(this BookingCreateDto dto)
    //{
    //    return new BookingCreateViewModel
    //    {
    //        SessionDetails = dto.SessionDetails.GetMainSessionDetailsViewModel(),
    //        Members = [..dto.Members.Select(m => new MemberLookupItem
    //        {
    //            Id = m.Id,
    //            Name = m.Name
    //        })]
    //    };
    //}


    public static BookingMainSessionDetailsViewModel GetMainSessionDetailsViewModel(this BookingMainSessionDetailsDto dtoQS)
    {
        return new BookingMainSessionDetailsViewModel
        {
            SessionId = dtoQS.SessionId,
            CategoryName = dtoQS.CategoryName,
            TrainerName = dtoQS.TrainerName,
            Capacity = dtoQS.Capacity,
            StartDate = dtoQS.StartDate,
            EndDate = dtoQS.EndDate
        };
    }

    public static BookingSessionMemberViewModel GetBookingMemberViewModel(
        this BookingSessionMemberDto dto,
        IUserTimeZoneService zoneService)
    {
        return new BookingSessionMemberViewModel
        {
            Id = dto.Id,
            MemberName = dto.MemberName,
            BookingDate = zoneService.ToUserTime(dto.BookingDate),
            IsAttended = dto.IsAttended
        };
    }
}
