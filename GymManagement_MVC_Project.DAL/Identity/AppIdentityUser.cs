using Microsoft.AspNetCore.Identity;

namespace GymManagement_MVC_Project.DAL.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FullName { get; set; } = default!;
}
