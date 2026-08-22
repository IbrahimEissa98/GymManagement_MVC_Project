namespace GymManagement_MVC_Project.PL.Helper;

public interface IUserTimeZoneService
{
    DateTime ToUserTime(DateTime utcDateTime);
}