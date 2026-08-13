using GymManagement_MVC_Project.BLL.Common;
using GymManagement_MVC_Project.BLL.DTOs.Home;

namespace GymManagement_MVC_Project.BLL.Services.Contracts;

public interface IHomeService
{
    Task<Result<HomeAnalyticsDto>> GetAnalyticsAsync(CancellationToken ct = default);
}
