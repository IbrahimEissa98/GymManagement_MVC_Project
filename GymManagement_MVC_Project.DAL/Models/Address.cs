using Microsoft.EntityFrameworkCore;

namespace GymManagement_MVC_Project.DAL.Models;

[Owned]
public class Address
{
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string BuildingNumber { get; set; } = default!;
}
