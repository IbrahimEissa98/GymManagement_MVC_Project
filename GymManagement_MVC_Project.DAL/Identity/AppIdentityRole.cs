using Microsoft.AspNetCore.Identity;

namespace GymManagement_MVC_Project.DAL.Identity;

public class AppIdentityRole : IdentityRole
{
    public string DisplayName { get; set; } = default!;
}
