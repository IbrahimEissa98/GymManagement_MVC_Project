namespace GymManagement_MVC_Project.DAL.Models;

public class ErrorViewModel
{
    public int StatusCode { get; set; }

    public string Title { get; set; } = "";

    public string Message { get; set; } = "";

    public string? Path { get; set; }

    public string? RequestId { get; set; }

    public bool ShowRequestId =>
        !string.IsNullOrWhiteSpace(RequestId);
}
