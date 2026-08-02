using GymManagement_MVC_Project.BLL.DTOs.HealthRecord;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.ViewModels.HealthRecord;
using GymManagement_MVC_Project.PL.ViewModels.Member;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class MembersController(IMemberService memberService) : Controller
{
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
        });

        return View(membersViewModel);
    }

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
}
