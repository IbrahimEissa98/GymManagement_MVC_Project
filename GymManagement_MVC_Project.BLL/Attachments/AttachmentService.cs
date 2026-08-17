using GymManagement_MVC_Project.BLL.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;

namespace GymManagement_MVC_Project.BLL.Attachments;

public class AttachmentService(IHostEnvironment hostEnv) : IAttachmentService
{
    private readonly string _root = Path.Combine(hostEnv.ContentRootPath, "Attachments");

    public async Task<Result<string>> SaveAsync(IFormFile formFile, string category, CancellationToken ct = default)
    {
        var validation = await ValidateImageAsync(formFile, ct);
        if (validation.IsFailure)
            return Result<string>.Failure(validation.Error!, ErrorType.Failure);

        var extension = NormalizedExtension(Path.GetExtension(formFile.FileName));

        var fileName = $"{Guid.NewGuid():N}{extension}";

        var dir = Path.Combine(_root, category);
        Directory.CreateDirectory(dir);

        try
        {
            await using var stream = new FileStream(Path.Combine(dir, fileName), FileMode.CreateNew, FileAccess.Write);
            await formFile.CopyToAsync(stream, ct);

            return Result<string>.Success($"{category}/{fileName}");
        }
        catch (Exception)
        {
            return Result<string>.Failure("Failed to upload photo.", ErrorType.Failure);
        }
    }

    public Task<Result<(Stream stream, string contentType)>> GetAsync(string storageKey, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            return Task.FromResult(Result<(Stream stream, string contentType)>.Failure("Invalid file name.", ErrorType.Failure));

        try
        {
            var path = ToFullPath(storageKey);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return Task.FromResult(Result<(Stream stream, string contentType)>.Failure("File not found.", ErrorType.Failure));

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            if (!stream.CanRead)
                return Task.FromResult(Result<(Stream stream, string contentType)>.Failure("Cannot open file.", ErrorType.Failure));

            var extension = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(extension))
                return Task.FromResult(Result<(Stream stream, string contentType)>.Failure("Invalid file extension.", ErrorType.Failure));
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
            return Task.FromResult(Result<(Stream stream, string contentType)>.Success((stream, contentType)));
        }
        catch (Exception)
        {
            return Task.FromResult(Result<(Stream stream, string contentType)>.Failure("Failed to delete file.", ErrorType.Failure));
        }
    }

    public Task<Result> DeleteAsync(string storageKey, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            return Task.FromResult(Result.Failure("Invalid file name.", ErrorType.Failure));

        try
        {
            var path = ToFullPath(storageKey);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return Task.FromResult(Result.Failure("File not found.", ErrorType.Failure));

            File.Delete(path);
            return Task.FromResult(Result.Success());
        }
        catch (Exception)
        {
            return Task.FromResult(Result.Failure("Failed to delete file.", ErrorType.Failure));
        }
    }


    private async Task<Result> ValidateImageAsync(IFormFile formFile, CancellationToken ct)
    {
        if (formFile is null)
            return Result.Failure("No file uploaded.", ErrorType.Failure);
        if (formFile.Length == 0)
            return Result.Failure("File is empty.", ErrorType.Failure);
        if (formFile.Length > AttachmentsRules.MaxSize)
            return Result.Failure("File size exceeded.", ErrorType.Failure);

        var extension = Path.GetExtension(formFile.FileName);
        if (extension is null || AttachmentsRules.allowedExtensions.Contains(extension))
            return Result.Failure("Invalid photo extension.", ErrorType.Validation);

        try
        {
            await using var stream = formFile.OpenReadStream();
            if (!stream.CanRead)
                return Result.Failure("Cannot read this photo.", ErrorType.Failure);
            var info = await Image.IdentifyAsync(stream, ct);
            if (info is null)
                return Result.Failure("Invalid image.", ErrorType.Failure);
            if (info.Width > 3000 || info.Height > 3000)
                return Result.Failure("Image out of dimensions.", ErrorType.Failure);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure("Invalid image.", ErrorType.Failure);
        }
    }

    private string NormalizedExtension(string ext)
        => string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ? ".jpg" : ext.ToLowerInvariant();

    private string ToFullPath(string storageKey)
        => Path.Combine(_root, storageKey.Replace('/', Path.DirectorySeparatorChar));
}
