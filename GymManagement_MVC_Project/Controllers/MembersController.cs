using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.ViewModels.HealthRecord;
using GymManagement_MVC_Project.PL.ViewModels.Member;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class MembersController(IMemberService memberService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var members = await memberService.GetAllAsync(ct);

        var membersViewModel = members.Select(m => new MemberIndexViewModel
        {
            Id = m.Id,
            PhotoUrl = m.PhotoUrl,
            Name = m.Name,
            Email = m.Email,
            Phone = m.Phone,
            Gender = m.Gender,
            IsDeleted = m.IsDeleted
        });

        return View(membersViewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MemberCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(model);

        var createDto = new MemberCreateDto
        {
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Gender = model.Gender,
            DateOfBirth = model.DateOfBirth,
            BuildingNumber = model.BuildingNumber,
            Street = model.Street,
            City = model.City,
            HealthRecord = new HealthRecordCreateDto
            {
                Height = model.HealthRecord.Height,
                Weight = model.HealthRecord.Weight,
                BloodType = model.HealthRecord.BloodType,
                Note = model.HealthRecord.Note
            }
        };

        var result = await memberService.CreateAsync(createDto, ct);

        if (result)
            TempData["SuccessMessage"] = "Member Created Successfully";
        else
            TempData["FailMessage"] = "Failed To Create Member";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var memberDto = await memberService.GetDetailsAsync(id, ct);

        if (memberDto is null) return NotFound();

        var memberViewModel = new MemberDetailsViewModel
        {
            Id = memberDto.Id,
            PhotoUrl = memberDto.PhotoUrl,
            Name = memberDto.Name,
            Email = memberDto.Email,
            Phone = memberDto.Phone,
            Gender = memberDto.Gender.ToString(),
            DateOfBirth = memberDto.DateOfBirth,
            Address = memberDto.Address,
            MembershipStartDate = memberDto.MembershipStartDate,
            MembershipEndDate = memberDto.MembershipEndDate,
            PlanName = memberDto.PlanName
        };

        return View(memberViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> HealthDetails(int id, CancellationToken ct)
    {
        var healthDto = await memberService.GetHealthRecordAsync(id, ct);

        if (healthDto is null) return NotFound();

        var healthViewModel = new HealthRecordDetailsViewModel
        {
            PhotoUrl = healthDto.PhotoUrl,
            Name = healthDto.Name,
            Height = healthDto.Height,
            Weight = healthDto.Weight,
            BloodType = healthDto.BloodType,
            Note = healthDto.Note
        };

        return View(healthViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var updateDto = await memberService.GetForUpdateAsync(id, ct);

        if (updateDto is null) return NotFound();

        var updateViewModel = new MemberToUpdateViewModel
        {
            Name = updateDto.Name,
            PhotoUrl = updateDto.PhotoUrl,
            Email = updateDto.Email,
            Phone = updateDto.Phone,
            BuildingNumber = updateDto.BuildingNumber,
            Street = updateDto.Street,
            City = updateDto.City
        };

        return View(updateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute]int id, MemberToUpdateViewModel updatedModel, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(updatedModel);

        var updatedDto = new MemberToUpdateDto
        {
            Name = updatedModel.Name,
            PhotoUrl = updatedModel.PhotoUrl,
            Email = updatedModel.Email,
            Phone = updatedModel.Phone,
            BuildingNumber = updatedModel.BuildingNumber,
            Street = updatedModel.Street,
            City = updatedModel.City
        };

        var result = await memberService.UpdateAsync(id, updatedDto, ct);

        if (result)
            TempData["SuccessMessage"] = "Member Updated Successfully";
        else
            TempData["FailMessage"] = "Failed To Update Member";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute]int id, CancellationToken ct)
    {
        var member = await memberService.GetForUpdateAsync(id, ct);

        if (member is null) return NotFound();

        var deleteViewModel = new MemberDeleteViewModel
        {
            Id = id,
            Name = member.Name
        };

        return View(deleteViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.DeleteAsync(id, ct);

        if (result)
            TempData["SuccessMessage"] = "Member Deactivated Successfully";
        else
            TempData["FailMessage"] = "Failed To Deactivate Member";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Activate([FromRoute]int id, CancellationToken ct)
    {
        var member = await memberService.GetForActivateAsync(id, ct);

        if (member is null) return NotFound();

        var activateViewModel = new MemberDeleteViewModel
        {
            Id = id,
            Name = member.Name
        };

        return View(activateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Activate")]
    public async Task<IActionResult> ActivateConfirm([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.ActivateAsync(id, ct);

        if (result)
            TempData["SuccessMessage"] = "Member Activated Successfully";
        else
            TempData["FailMessage"] = "Failed To Activate Member";

        return RedirectToAction(nameof(Index));
    }
}
