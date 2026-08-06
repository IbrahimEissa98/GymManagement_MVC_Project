using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.BLL.DTOs.Trainer;

public class TrainerCreateDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = default!;
    public string BuildingNumber { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string Specialties { get; set; } = default!;
}
