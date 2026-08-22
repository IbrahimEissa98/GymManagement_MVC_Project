using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.Extensions.Mapping;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class SessionsController(
    ISessionService sessionService,
    IUserTimeZoneService zoneService
    ) : Controller
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly IUserTimeZoneService _zoneService = zoneService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var sessions = await _sessionService.GetAllAsync(ct);

        IReadOnlyList<SessionIndexViewModel> indexVM = [.. sessions.Value!.Select(s => s.GetSessionIndexViewModel(_zoneService))];

        return View(indexVM);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var sessionDto = await _sessionService.GetDetailsAsync(id, ct);

        if (sessionDto.IsFailure)
            return NotFound(sessionDto.Error);

        return View(sessionDto.Value!.GetSessionDetailsViewModel(_zoneService));
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var categories = await _sessionService.GetAllCategoryLookupItems(ct);
        return View(new SessionCreateViewModel { Categories = categories.Value! });
    }

    [HttpGet]
    public async Task<IActionResult> GetTrainersByCategory(int categoryId, CancellationToken ct)
    {
        var trainers = await _sessionService.GetAllTrainersByCategoryId(categoryId, ct);

        return Json(trainers.Value);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SessionCreateViewModel createModel, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadAllCategories(createModel, ct);
            await LoadTrainersForCategory(createModel, ct);
            return View(createModel);
        }

        var result = await _sessionService.CreateAsync(createModel.GetSessionCreateDto(), ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Session Created Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                await LoadAllCategories(createModel, ct);
                await LoadTrainersForCategory(createModel, ct);
                return View(createModel);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await _sessionService.GetForEditAsync(id, ct);
        if (result.IsFailure)
        {
            TempData["FailMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }

        var editVM = result.Value!.GetSessionEditViewModel(_zoneService);
        editVM.Trainers = (await _sessionService.GetAllTrainersByCategoryId(result.Value!.CategoryId, ct)).Value!;

        return View(editVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, SessionEditViewModel editedVM, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(editedVM);

        var result = await _sessionService.EditAsync(id, editedVM.GetSessionEditDto(), ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Session updated Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                editedVM.Trainers = (await _sessionService.GetAllTrainersByCategoryId(editedVM.CategoryId, ct)).Value!;
                return View(editedVM);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var sessionDto = await _sessionService.GetForDeleteOrActivateAsync(id, ct: ct);

        if (sessionDto.IsFailure)
        {
            TempData["FailMessage"] = sessionDto.Error;
            return RedirectToAction(nameof(Index));
        }

        var delVM = new SessionDeleteViewModel
        {
            CategoryName = sessionDto.Value!.CategoryName,
            TrainerName = sessionDto.Value!.TrainerName
        };
        return View(delVM);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm(int id, CancellationToken ct)
    {
        var result = await _sessionService.DeleteAsync(id, ct);
        if (result.IsFailure)
            TempData["FailMessage"] = result.Error;
        else
            TempData["SuccessMessage"] = "Session Deactivated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var sessionDto = await _sessionService.GetForDeleteOrActivateAsync(id, isDelete: false, ct: ct);

        if (sessionDto.IsFailure)
        {
            TempData["FailMessage"] = sessionDto.Error;
            return RedirectToAction(nameof(Index));
        }

        var delVM = new SessionDeleteViewModel
        {
            CategoryName = sessionDto.Value!.CategoryName,
            TrainerName = sessionDto.Value!.TrainerName
        };
        return View(delVM);
    }

    [HttpPost]
    [ActionName("Activate")]
    public async Task<IActionResult> ActivateConfirm(int id, CancellationToken ct)
    {
        var result = await _sessionService.ActivateAsync(id, ct);
        if (result.IsFailure)
            TempData["FailMessage"] = result.Error;
        else
            TempData["SuccessMessage"] = "Session activated successfully.";
        return RedirectToAction(nameof(Index));
    }


    private async Task LoadAllCategories(SessionCreateViewModel createModel, CancellationToken ct = default)
    {
        createModel.Categories = (await _sessionService.GetAllCategoryLookupItems(ct)).Value!;
    }
    private async Task LoadTrainersForCategory(SessionCreateViewModel createModel, CancellationToken ct = default)
    {
        createModel.Trainers = (await _sessionService.GetAllTrainersByCategoryId(createModel.CategoryId, ct)).Value!;
    }
}
