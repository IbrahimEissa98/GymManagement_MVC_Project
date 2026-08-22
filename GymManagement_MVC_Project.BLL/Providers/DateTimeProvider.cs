using GymManagement_MVC_Project.BLL.Providers.Contracts;

namespace GymManagement_MVC_Project.BLL.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
