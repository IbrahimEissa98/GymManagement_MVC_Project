using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class HomeController(IHomeService homeService) : Controller
{
    private readonly IHomeService _homeService = homeService;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var analytics = await _homeService.GetAnalyticsAsync(ct);
        var model = new HomeAnalyticsViewModel
        {
            TotalMembers = analytics.Value!.TotalMembers,
            ActiveMembers = analytics.Value!.ActiveMembers,
            TotalTrainers = analytics.Value!.TotalTrainers,
            UpcomingSessions = analytics.Value!.UpcomingSessions,
            OngoingSessions = analytics.Value!.OngoingSessions,
            CompletedSessions = analytics.Value!.CompletedSessions
        };
        return View(model);
    }

}
