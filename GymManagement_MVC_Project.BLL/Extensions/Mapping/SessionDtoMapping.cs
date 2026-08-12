using GymManagement_MVC_Project.BLL.DTOs.Session;
using GymManagement_MVC_Project.BLL.DTOs.Session.Enums;
using GymManagement_MVC_Project.DAL.Models;
using GymManagement_MVC_Project.DAL.Queries.DTOs;

namespace GymManagement_MVC_Project.BLL.Extensions.Mapping;

public static class SessionDtoMapping
{
    public static SessionIndexDto GetSessionIndexDto(this SessionIndexDtoQS indexDtoQS)
    {
        return new SessionIndexDto
        {
            Id = indexDtoQS.Id,
            CategoryName = indexDtoQS.CategoryName,
            Status = GetSessionStatus(indexDtoQS.StartDate, indexDtoQS.EndDate),
            Description = indexDtoQS.Description,
            TrainerName = indexDtoQS.TrainerName,
            StartDate = indexDtoQS.StartDate,
            EndDate = indexDtoQS.EndDate,
            Capacity = indexDtoQS.Capacity,
            BookedCount = indexDtoQS.BookedCount,
            IsDeleted = indexDtoQS.IsDeleted
        };
    }

    public static Session GetSession(this SessionCreateDto createDto)
    {
        return new Session
        {
            Description = createDto.Description,
            Capacity = createDto.Capacity,
            TrainerId = createDto.TrainerId,
            CategoryId = createDto.CategoryId,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
        };
    }

    public static SessionEditDto GetSessionEditDto(this Session session)
    {
        return new SessionEditDto
        {
            TrainerId = session.TrainerId,
            CategoryId = session.CategoryId,
            Description = session.Description,
            StartDate = session.StartDate,
            EndDate = session.EndDate
        };
    }

    private static SessionStatus GetSessionStatus(DateTime startDate, DateTime endDate)
    {
        if (startDate > DateTime.UtcNow)
            return SessionStatus.Upcoming;
        else if (endDate >= DateTime.UtcNow)
            return SessionStatus.Ongoing;
        else
            return SessionStatus.Completed;
    }
}
