namespace GymManagement_MVC_Project.BLL.DTOs.Trainer;

public class TrainerIndexDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Specialize { get; set; } = default!;
    public bool IsDeleted { get; set; }
}
