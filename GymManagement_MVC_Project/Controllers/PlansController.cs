using GymManagement_MVC_Project.BLL.DTOs.Plan;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.ViewModels.Plan;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class PlansController(IPlanService planService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var plans = await planService.GetAllAsync();


        var plansViewModel = plans.Value?.Select(p => new PlanIndexViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            DurationDays = p.DurationDays,
            IsActive = p.IsActive
        });

        return View(plansViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await planService.GetDetailsAsync(id);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var plan = result.Value!;

        var planViewModel = new PlanIndexViewModel
        {
            Id = plan.Id,
            Name = plan.Name,
            Price = plan.Price,
            Description = plan.Description,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive
        };

        return View(planViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await planService.GetForEditAsync(id, ct);

        if (!result.IsSuccess)
        {
            TempData["FailMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        var planDto = result.Value!;

        var editViewModel = new PlanEditViewModel
        {
            Name = planDto.Name,
            Price = planDto.Price,
            Description = planDto.Description,
            DurationDays = planDto.DurationDays
        };

        return View(editViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, PlanEditViewModel editViewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(editViewModel);

        var editDto = new PlanEditDto
        {
            Name = editViewModel.Name,
            Price = editViewModel.Price,
            Description = editViewModel.Description,
            DurationDays = editViewModel.DurationDays
        };

        var result = await planService.EditAsync(id, editDto, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Plan Updated Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActivate(int id, CancellationToken ct)
    {
        var result = await planService.ToggleActivationAsync(id, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Plan Activation Toggled Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }
}
