using GymManagement_MVC_Project.BLL.DTOs.Session;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.ViewModels.Sessions;

namespace GymManagement_MVC_Project.PL.Extensions.Mapping;

public static class SessionVMMapping
{
    public static SessionIndexViewModel GetSessionIndexViewModel(
        this SessionIndexDto indexDto,
        IUserTimeZoneService zoneService)
    {
        return new SessionIndexViewModel
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

    public static SessionDetailsViewModel GetSessionDetailsViewModel(
        this SessionIndexDto indexDto,
        IUserTimeZoneService zoneService)
    {
        return new SessionDetailsViewModel
        {
            Id = indexDto.Id,
            CategoryName = indexDto.CategoryName,
            Status = indexDto.Status,
            Description = indexDto.Description,
            TrainerName = indexDto.TrainerName,
            StartDate = zoneService.ToUserTime(indexDto.StartDate).ToString(),
            EndDate = zoneService.ToUserTime(indexDto.EndDate).ToString(),
            Duration = indexDto.EndDate - indexDto.StartDate,
            Capacity = indexDto.Capacity,
            BookedCount = indexDto.BookedCount
        };
    }

    public static SessionCreateDto GetSessionCreateDto(this SessionCreateViewModel model)
    {
        return new SessionCreateDto
        {
            CategoryId = model.CategoryId,
            TrainerId = model.TrainerId,
            StartDate = model.StartDate.ToUniversalTime(),
            EndDate = model.EndDate.ToUniversalTime(),
            Capacity = model.Capacity,
            Description = model.Description
        };
    }

    public static SessionEditViewModel GetSessionEditViewModel(this SessionEditDto editDto, IUserTimeZoneService zoneService)
    {
        return new SessionEditViewModel
        {
            TrainerId = editDto.TrainerId,
            CategoryId = editDto.CategoryId,
            Description = editDto.Description,
            StartDate = zoneService.ToUserTime(editDto.StartDate),
            EndDate = zoneService.ToUserTime(editDto.EndDate)
        };
    }

    public static SessionEditDto GetSessionEditDto(this SessionEditViewModel editVM)
    {
        return new SessionEditDto
        {
            CategoryId = editVM.CategoryId,
            TrainerId = editVM.TrainerId,
            StartDate = editVM.StartDate.ToUniversalTime(),
            EndDate = editVM.EndDate.ToUniversalTime(),
            Description = editVM.Description
        };
    }
}
