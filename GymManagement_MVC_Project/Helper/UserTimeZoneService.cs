using TimeZoneConverter;

namespace GymManagement_MVC_Project.PL.Helper;

public class UserTimeZoneService(IHttpContextAccessor httpContextAccessor) : IUserTimeZoneService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DateTime ToUserTime(DateTime utcDateTime)
    {
        var timeZoneId = _httpContextAccessor
            .HttpContext?
            .Request
            .Cookies["TimeZone"];

        if (string.IsNullOrWhiteSpace(timeZoneId))
            timeZoneId = "UTC";

        //var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc),
            timeZone);
    }
}
