namespace GymManagement_MVC_Project.BLL.Providers.Contracts;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}
