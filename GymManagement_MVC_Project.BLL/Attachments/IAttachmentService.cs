using GymManagement_MVC_Project.BLL.Common;
using Microsoft.AspNetCore.Http;

namespace GymManagement_MVC_Project.BLL.Attachments;

public interface IAttachmentService
{
    Task<Result<string>> SaveAsync(IFormFile formFile, string category, CancellationToken ct = default);
    Task<Result<(Stream stream, string contentType)>> GetAsync(string storageKey, CancellationToken ct = default);
    Task<Result> DeleteAsync(string storageKey, CancellationToken ct = default);
}
