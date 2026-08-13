using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Home;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;

namespace GymManagement_MVC_Project.BLL.Services;

internal class HomeService(
    IUnitOfWork unitOfWork,
    IDateTimeProvider clock) : IHomeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _clock = clock;

    public async Task<Result<HomeAnalyticsDto>> GetAnalyticsAsync(CancellationToken ct = default)
    {
        var totalMembers = await _unitOfWork.Members.GetCountAsync(ct: ct);
        var activeMembers = await _unitOfWork.Members.GetCountAsync(m => m.Memberships.Any(ms => ms.StartDate < _clock.UtcNow && ms.EndDate > _clock.UtcNow), ct: ct);
        var totalTrainers = await _unitOfWork.Trainers.GetCountAsync(ct: ct);
        var upcomingSessions = await _unitOfWork.Sessions.GetCountAsync(s => s.StartDate > _clock.UtcNow, ct: ct);
        var ongoingSessions = await _unitOfWork.Sessions.GetCountAsync(s => s.StartDate <= _clock.UtcNow && s.EndDate > _clock.UtcNow, ct: ct);
        var completedSessions = await _unitOfWork.Sessions.GetCountAsync(s => s.EndDate <= _clock.UtcNow, ct: ct);

        return Result<HomeAnalyticsDto>.Success(new HomeAnalyticsDto
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            TotalTrainers = totalTrainers,
            UpcomingSessions = upcomingSessions,
            OngoingSessions = ongoingSessions,
            CompletedSessions = completedSessions
        });
    }
}
