namespace GymManagementProject.Models.Interceptors;

public interface IHasTimestamps
{
    DateTime CreatedAt { get; set; }

    DateTime? UpdatedAt { get; set; }
}
