using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.Extensions.Mapping;
using GymManagement_MVC_Project.PL.ViewModels.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class MembershipsController(IMembershipService membershipService) : Controller
{
    private readonly IMembershipService _membershipService = membershipService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await _membershipService.GetAllAsync(ct);
        IReadOnlyList<MembershipIndexViewModel> membershipsVM = [.. result.Value!.Select(ms => ms.GetViewModel())];
        return View(membershipsVM);
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        var members = await _membershipService.GetAllUnsubscribedMembersAsync(ct);
        var plans = await _membershipService.GetAllActivePlansAsync(ct);
        var createVM = new MembershipCreateViewModel
        {
            Members = members.Value!,
            Plans = plans.Value!
        };
        return View(createVM);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MembershipCreateViewModel createVM, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadMembersAndPlans(createVM, ct);
            return View(createVM);
        }

        var result = await _membershipService.CreateAsync(createVM.GetCreateDto(), ct);
        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Membership Created Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                await LoadMembersAndPlans(createVM, ct);
                return View(createVM);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var result = await _membershipService.CancelAsync(id, ct);
        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Membership Canceled Successfully";
        else
        {
            TempData["FailMessage"] = result.Error;
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadMembersAndPlans(MembershipCreateViewModel createVM, CancellationToken ct)
    {
        var members = await _membershipService.GetAllUnsubscribedMembersAsync(ct);
        var plans = await _membershipService.GetAllActivePlansAsync(ct);
        createVM.Members = members.Value!;
        createVM.Plans = plans.Value!;
    }
}
