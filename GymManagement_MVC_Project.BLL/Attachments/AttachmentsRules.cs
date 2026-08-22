namespace GymManagement_MVC_Project.BLL.Attachments;

public class AttachmentsRules
{
    public const long MaxSize = 5 * 1024 * 1024;  // 5MB
    public static readonly HashSet<string> allowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png"
    };
}
