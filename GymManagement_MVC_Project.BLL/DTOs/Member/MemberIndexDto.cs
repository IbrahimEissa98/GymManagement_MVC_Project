namespace GymManagement_MVC_Project.BLL.DTOs.Member;

public class MemberIndexDto
{
    public int Id { get; set; }
    public string? PhotoUrl { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public bool IsDeleted { get; set; }
}



//public class MemberIndexViewModel
//{
//    public int Id { get; set; }
//    public string? Photo { get; set; } = default!;
//    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
//    public string Name { get; set; } = default!;

//    [EmailAddress]
//    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
//    public string Email { get; set; } = default!;

//    [Phone]
//    [RegularExpression(@"^01[0125]\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
//    public string Phone { get; set; } = default!;

//    [AgeRange(ErrorMessage = "Invalid Entered date of birth.")]
//    public DateOnly DateOfBirth { get; set; }

//    [EnumChoice<GenderTypes>]
//    public GenderTypes Gender { get; set; }
//    public Address Address { get; set; } = default!;
//}
