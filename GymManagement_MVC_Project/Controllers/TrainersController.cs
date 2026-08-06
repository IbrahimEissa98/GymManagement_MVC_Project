using GymManagement_MVC_Project.BLL.DTOs.Trainer;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Models.Enums;
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

        IReadOnlyList<TrainerIndexViewModel> trainersViewModel = [..trainers.Select(t => new TrainerIndexViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Email = t.Email,
            Phone = t.Phone,
            Specialize = t.Specialize,
            IsDeleted = t.IsDeleted
        })];

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

        var createDto = new TrainerCreateDto
        {
            Name = createViewModel.Name,
            Email = createViewModel.Email,
            Phone = createViewModel.Phone,
            DateOfBirth = createViewModel.DateOfBirth,
            Gender = createViewModel.Gender,
            Specialties = createViewModel.Specialties,
            City = createViewModel.City,
            Street = createViewModel.Street,
            BuildingNumber = createViewModel.BuildingNumber
        };

        var result = await _trainerService.CreateAsync(createDto, ct);

        if (result)
            TempData["SuccessMessage"] = "Trainer Created Successfully";
        else
            TempData["FailMessage"] = "Failed To Create Trainer";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var trainerDto = await _trainerService.GetDetailsAsync(id, ct);

        if (trainerDto is null)
            return NotFound();

        var detailsViewModel = new TrainerDetailsViewModel
        {
            Name = trainerDto.Name,
            Email = trainerDto.Email,
            Phone = trainerDto.Phone,
            Specialties = trainerDto.Specialties.ToString(),
            DateOfBirth = trainerDto.DateOfBirth,
            Gender = trainerDto.Gender,
            Address = trainerDto.Address
        };

        return View(detailsViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var editDto = await _trainerService.GetForEditAsync(id, ct);

        if (editDto is null)
            return BadRequest();

        if (!Enum.TryParse(editDto.Specialties, true, out TrainerSpecialties specialties))
            ModelState.AddModelError("specialties", "Invalid specialties");

        var editViewModel = new TrainerEditViewModel
        {
            Name = editDto.Name,
            Email = editDto.Email,
            Phone = editDto.Phone,
            BuildingNumber = editDto.BuildingNumber,
            Street = editDto.Street,
            City = editDto.City,
            Specialties = specialties
        };

        return View(editViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, TrainerEditViewModel editViewModel, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(editViewModel);

        var editDto = new TrainerEditDto
        {
            Name = editViewModel.Name,
            Email = editViewModel.Email,
            Phone = editViewModel.Phone,
            BuildingNumber = editViewModel.BuildingNumber,
            Street = editViewModel.Street,
            City = editViewModel.City,
            Specialties = editViewModel.Specialties.ToString()
        };

        var result = await _trainerService.EditAsync(id, editDto, ct);

        if (result)
            TempData["SuccessMessage"] = "Trainer Updated Successfully";
        else
            TempData["FailMessage"] = "Failed To Update Trainer";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetForDeleteOrRestoreAsync(id, ct: ct);

        if (trainer is null)
            return NotFound();

        var deleteViewModel = new TrainerDeleteViewModel
        {
            Id = trainer.Id,
            Name = trainer.Name
        };

        return View(deleteViewModel);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> ConfirmDelete(int id, CancellationToken ct)
    {
        var result = await _trainerService.DeleteAsync(id, ct);

        if (result)
            TempData["SuccessMessage"] = "Trainer Deactivated Successfully";
        else
            TempData["FailMessage"] = "Failed To Deactivate Trainer";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetForDeleteOrRestoreAsync(id, false, ct: ct);

        if (trainer is null)
            return NotFound();

        var deleteViewModel = new TrainerDeleteViewModel
        {
            Id = trainer.Id,
            Name = trainer.Name
        };

        return View(deleteViewModel);
    }

    [HttpPost]
    [ActionName("Activate")]
    public async Task<IActionResult> ConfirmActivate(int id, CancellationToken ct)
    {
        var result = await _trainerService.ActivateAsync(id, ct);

        if (result)
            TempData["SuccessMessage"] = "Trainer Activated Successfully";
        else
            TempData["FailMessage"] = "Failed To Activate Trainer";

        return RedirectToAction(nameof(Index));
    }
}
