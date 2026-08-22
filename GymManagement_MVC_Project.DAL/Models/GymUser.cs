using GymManagement_MVC_Project.DAL.Models.Enums;
using GymManagement_MVC_Project.DAL.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.DAL.Models;

public abstract class GymUser : BaseEntity
{
    public string Name { get; set; } = default!;

    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = default!;

    [Phone]
    [RegularExpression(@"^01[0125]\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
    public string Phone { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }

    [EnumChoice<GenderTypes>]
    public GenderTypes Gender { get; set; }
    public Address Address { get; set; } = default!;

    //public DateTime JoinDate { get; set; } // CreatedAt
}
