using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.BLL.ViewModels.Member;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement_MVC_Project.PL.Controllers;

public class MembersController(IMemberService memberService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var members = await memberService.GetAllAsync(ct);

        return View(members);
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

        var result = await memberService.CreateAsync(model, ct);

        if (result)
            TempData["SuccessMessage"] = "Success";
        else
            TempData["FailMessage"] = "Failed";

        return RedirectToAction(nameof(Index));
    }
}
