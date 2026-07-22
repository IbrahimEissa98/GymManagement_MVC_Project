using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Repositories.Plans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.PL.Controllers;

public class PlanController : Controller
{
    private readonly IPlanRepository _planRepo;

    public PlanController(IPlanRepository planRepo)
    {
        _planRepo = planRepo;
    }

    public async Task<IActionResult> Index()
    {
        var plans = await _planRepo.GetAllPlansAsync();

        return View(plans);
    }

    //[HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        if(id <= 0)
        {
            return NotFound();
        }
        var plan = await _planRepo.GetPlanByIdAsync(id);

        if (plan is null)
            return RedirectToAction(nameof(Index));
        
        return View(plan);
    }
}
