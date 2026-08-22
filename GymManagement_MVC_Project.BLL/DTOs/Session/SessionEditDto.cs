namespace GymManagement_MVC_Project.BLL.DTOs.Session;

public class SessionEditDto
{
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TrainerId { get; set; }
    public int CategoryId { get; set; }
}
