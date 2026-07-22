namespace GymManagement_MVC_Project.DAL.Models.Interceptors;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}
