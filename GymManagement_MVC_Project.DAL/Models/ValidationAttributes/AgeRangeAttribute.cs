using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.DAL.Models.ValidationAttributes;

public class AgeRangeAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateOnly dob)
            return false;

        var today = DateOnly.FromDateTime(DateTime.Now);

        return dob <= today.AddYears(-2)
            && dob >= today.AddYears(-100);
    }
}
