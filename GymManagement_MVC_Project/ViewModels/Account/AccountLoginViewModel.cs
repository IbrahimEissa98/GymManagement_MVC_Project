using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.PL.ViewModels.Account;

public class AccountLoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [DisplayName("Email Address")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}
