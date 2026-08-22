using GymManagement_MVC_Project.DAL.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement_MVC_Project.PL.Controllers;

public class ErrorController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        var model = new ErrorViewModel
        {
            StatusCode = 500,
            Title = "Something went wrong",
            Message = "An unexpected error occurred while processing your request.",
            Path = feature?.Path,
            RequestId = HttpContext.TraceIdentifier
        };

        return View(model);
    }

    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        var model = new ErrorViewModel
        {
            StatusCode = statusCode,
            RequestId = HttpContext.TraceIdentifier
        };

        switch (statusCode)
        {
            case 400:
                model.Title = "Bad Request";
                model.Message = "The request could not be processed.";
                break;

            case 401:
                model.Title = "Unauthorized";
                model.Message = "You must login to access this page.";
                break;

            case 403:
                model.Title = "Access Denied";
                model.Message = "You don't have permission to access this page.";
                break;

            case 404:
                model.Title = "Page Not Found";
                model.Message = "The page you are looking for doesn't exist.";
                break;

            default:
                model.Title = "Unexpected Error";
                model.Message = "Something went wrong.";
                break;
        }

        return View("Error", model);
    }
}