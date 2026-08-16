using GymManagement_MVC_Project.BLL.DTOs.Booking;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.PL.Extensions.Mapping;
using GymManagement_MVC_Project.PL.Helper;
using GymManagement_MVC_Project.PL.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class BookingsController(
    IBookingService bookingService,
    IUserTimeZoneService zoneService
    ) : Controller
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IUserTimeZoneService _zoneService = zoneService;

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var sessions = await _bookingService.GetAllSessionsAsync(ct);
        IReadOnlyList<BookingIndexViewModel> indexVM = [.. sessions.Value!.Select(s => s.GetBookingIndexViewModel(_zoneService))];
        return View(indexVM);
    }

    [HttpGet]
    public async Task<IActionResult> ViewUpcoming([FromRoute] int Id, CancellationToken ct)
    {
        var bookings = await _bookingService.GetSessionBookings(Id, ct);
        if (bookings.IsFailure)
        {
            TempData["FailMessage"] = bookings.Error;
            return RedirectToAction(nameof(Index));
        }
        var bookingsVM = bookings.Value!.GetViewModel(_zoneService);
        return View(bookingsVM);
    }

    [HttpGet]
    public async Task<IActionResult> ViewOngoing([FromRoute] int Id, CancellationToken ct)
    {
        var bookings = await _bookingService.GetSessionBookings(Id, ct);
        if (bookings.IsFailure)
        {
            TempData["FailMessage"] = bookings.Error;
            return RedirectToAction(nameof(Index));
        }
        var bookingsVM = bookings.Value!.GetViewModel(_zoneService);
        return View(bookingsVM);
    }

    [HttpGet("Bookings/ViewUpcoming/{id}/Create")]
    public async Task<IActionResult> Create(int id, CancellationToken ct)
    {
        var sessionDetails = await _bookingService.GetMainSessionDetailsAsync(id, ct);
        if (sessionDetails.IsFailure)
        {
            TempData["FailMessage"] = sessionDetails.Error;
            return RedirectToAction(nameof(Index));
        }

        var members = await _bookingService.GetMembersToCreateAsync(id, ct);
        if (members.IsFailure)
        {
            TempData["FailMessage"] = members.Error;
            return RedirectToAction(nameof(ViewUpcoming), new { id = sessionDetails.Value!.SessionId });
        }

        var bookedCount = await _bookingService.GetSessionBookedMembersAsync(id, ct);
        if (bookedCount.Value!.Count >= sessionDetails.Value!.Capacity)
        {
            TempData["FailMessage"] = "This session reached capacity limit";
            return RedirectToAction(nameof(ViewUpcoming), new { id = sessionDetails.Value!.SessionId });
        }

        var createBooking = new BookingCreateViewModel
        {
            SessionDetails = sessionDetails.Value!.GetMainSessionDetailsViewModel(),
            Members = members.Value!
        };

        return View(createBooking);
    }

    [HttpPost("Bookings/ViewUpcoming/{Id}/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadSessionDetails(model, ct);
            await LoadAvailableMembers(model, ct);
            return View(model);
        }

        var result = await _bookingService.CreateAsync(new BookingCreateDto
        {
            MemberId = model.MemberId,
            SessionId = model.SessionId
        }, ct);

        if (result.IsSuccess)
            TempData["SuccessMessage"] = "Booking Created Successfully";
        else
        {
            if (result.ErrorKey is not null && result.Error is not null)
            {
                ModelState.AddModelError(result.ErrorKey, result.Error);
                await LoadSessionDetails(model, ct);
                await LoadAvailableMembers(model, ct);
                return View(model);
            }
            else
                TempData["FailMessage"] = result.Error;
        }

        return RedirectToAction(nameof(ViewUpcoming), new { id = model.SessionId });
    }

    [HttpPost("Bookings/ViewUpcoming/{id}/Cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel([FromRoute] int id, [FromQuery] int bookingId, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            TempData["FailMessage"] = "Failed cancelation, Please try again.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _bookingService.CancelAsync(bookingId, id, ct);
        if (result.IsFailure)
        {
            TempData["FailMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }
        TempData["SuccessMessage"] = "Booking canceled successfully.";
        return RedirectToAction(nameof(ViewUpcoming), new { id });
    }

    [HttpPost("Bookings/ViewUpcoming/{id}/Attend")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Attend([FromRoute] int id, [FromQuery] int bookingId, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            TempData["FailMessage"] = "Failed attendance, Please try again.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _bookingService.AttendAsync(bookingId, id, ct);
        if (result.IsFailure)
        {
            TempData["FailMessage"] = result.Error;
            return RedirectToAction(nameof(Index));
        }
        TempData["SuccessMessage"] = "Member attend successfully.";
        return RedirectToAction(nameof(ViewOngoing), new { id });
    }

    private async Task LoadAvailableMembers(BookingCreateViewModel model, CancellationToken ct = default)
    {
        model.Members = (await _bookingService.GetMembersToCreateAsync(model.SessionId, ct)).Value!;
    }
    private async Task LoadSessionDetails(BookingCreateViewModel model, CancellationToken ct = default)
    {
        model.SessionDetails = (await _bookingService.GetMainSessionDetailsAsync(model.SessionId, ct)).Value!.GetMainSessionDetailsViewModel();
    }
}
