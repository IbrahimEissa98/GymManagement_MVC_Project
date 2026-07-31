using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.PL.Controllers;

public class PlansController(IPlanRepository planRepo) : Controller
{
    public async Task<IActionResult> Index()
    {
        var plans = await planRepo.GetAllAsync();

        return View(plans);
    }

    //[HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        if(id <= 0)
        {
            return NotFound();
        }
        var plan = await planRepo.GetByIdAsync(id);

        if (plan is null)
            return RedirectToAction(nameof(Index));
        
        return View(plan);
    }
}
