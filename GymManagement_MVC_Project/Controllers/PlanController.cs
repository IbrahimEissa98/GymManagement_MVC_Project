using GymManagementProject.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementProject.Controllers;

public class PlanController : Controller
{
    private readonly GymDbContext _gymDbContext = new GymDbContext();

    public async Task<IActionResult> Index()
    {
        var plans = await _gymDbContext.Plans.ToListAsync();

        return View(plans);
    }

    //[HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        if(id <= 0)
        {
            return NotFound();
        }
        var plan = await _gymDbContext.Plans.FindAsync(id);

        if (plan is null)
            return RedirectToAction(nameof(Index));
        
        return View(plan);
    }
}
