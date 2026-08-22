using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.Extensions.Mapping;
using GymManagement_MVC_Project.PL.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class TrainersController(ITrainerService trainerService) : Controller
{
    private readonly ITrainerService _trainerService = trainerService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var trainers = await _trainerService.GetAllAsync(ct);

        IReadOnlyList<TrainerIndexViewModel> trainersViewModel = [.. trainers.Value!.Select(t => t.GetTrainerIndexVM())];

        return View(trainersViewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TrainerCreateViewModel createViewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(createViewModel);

        var createDto = createViewModel.GetTrainerCreateDto();

        var result = await _trainerService.CreateAsync(createDto, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Trainer Created Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                return View(createViewModel);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var trainerDto = await _trainerService.GetDetailsAsync(id, ct);

        if (!trainerDto.IsSuccess)
            return NotFound(trainerDto.Error);

        var detailsViewModel = trainerDto.Value!.GetTrainerDetailsVM();

        return View(detailsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var editDto = await _trainerService.GetForEditAsync(id, ct);

        if (!editDto.IsSuccess)
            return BadRequest(editDto.Error);

        var editViewModel = editDto.Value!.GetTrainerEditVM();

        return View(editViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, TrainerEditViewModel editViewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(editViewModel);

        var editDto = editViewModel.GetTrainerEditDto();

        var result = await _trainerService.EditAsync(id, editDto, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Trainer Updated Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                return View(editViewModel);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetForDeleteOrRestoreAsync(id, ct: ct);

        if (!trainer.IsSuccess)
            return NotFound(trainer.Error);

        var deleteViewModel = trainer.Value!.GetTrainerDeleteVM();

        return View(deleteViewModel);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int id, CancellationToken ct)
    {
        var result = await _trainerService.DeleteAsync(id, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Trainer Deactivated Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetForDeleteOrRestoreAsync(id, false, ct: ct);

        if (!trainer.IsSuccess)
            return NotFound(trainer.Error);

        var deleteViewModel = trainer.Value!.GetTrainerDeleteVM();

        return View(deleteViewModel);
    }

    [HttpPost]
    [ActionName("Activate")]
    public async Task<IActionResult> ConfirmActivate(int id, CancellationToken ct)
    {
        var result = await _trainerService.ActivateAsync(id, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Trainer Activated Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }
}
