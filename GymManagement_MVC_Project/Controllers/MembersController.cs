using AutoMapper;
using GymManagement_MVC_Project.BLL.Attachments;
using GymManagement_MVC_Project.BLL.DTOs.Member;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.ViewModels.HealthRecord;
using GymManagement_MVC_Project.PL.ViewModels.Member;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class MembersController(
    IMemberService memberService,
    IMapper mapper,
    IAttachmentService attachmentService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly IAttachmentService _attachmentService = attachmentService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var members = await memberService.GetAllAsync(ct);

        var membersViewModel = _mapper.Map<IReadOnlyList<MemberIndexViewModel>>(members.Value);

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

        var result = await memberService.CreateAsync(_mapper.Map<MemberCreateDto>(model), ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Member Created Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                return View(model);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var result = await memberService.GetDetailsAsync(id, ct);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var memberViewModel = _mapper.Map<MemberDetailsViewModel>(result.Value);

        return View(memberViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> HealthDetails(int id, CancellationToken ct)
    {
        var result = await memberService.GetHealthRecordAsync(id, ct);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var healthViewModel = _mapper.Map<HealthRecordDetailsViewModel>(result.Value);

        return View(healthViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var result = await memberService.GetForUpdateAsync(id, ct);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var updateViewModel = _mapper.Map<MemberToUpdateViewModel>(result.Value);

        return View(updateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] int id, MemberToUpdateViewModel updatedModel, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return View(updatedModel);

        var updatedDto = _mapper.Map<MemberToUpdateDto>(updatedModel);

        var result = await memberService.UpdateAsync(id, updatedDto, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Member Updated Successfully";
        else
        {
            if (result.ErrorKey is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error!);
                return View(updatedModel);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.GetForUpdateAsync(id, ct);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var deleteViewModel = new MemberDeleteViewModel
        {
            Id = id,
            Name = result.Value!.Name
        };

        return View(deleteViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirm([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.DeleteAsync(id, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Member Deactivated Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Activate([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.GetForActivateAsync(id, ct);

        if (!result.IsSuccess)
            return NotFound(result.Error);

        var activateViewModel = new MemberDeleteViewModel
        {
            Id = id,
            Name = result.Value!.Name
        };

        return View(activateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Activate")]
    public async Task<IActionResult> ActivateConfirm([FromRoute] int id, CancellationToken ct)
    {
        var result = await memberService.ActivateAsync(id, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Member Activated Successfully";
        else
            TempData["FailMessage"] = result.Error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Photo(string storageKey)
    {
        var stream = await _attachmentService.GetAsync(storageKey);
        if (stream.IsFailure || stream is null)
            return NotFound();

        return File(stream.Value.stream, stream.Value.contentType);
    }
}
