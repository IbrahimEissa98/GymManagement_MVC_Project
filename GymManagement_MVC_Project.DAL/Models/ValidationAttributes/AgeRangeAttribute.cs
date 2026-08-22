using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.DAL.Models.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
public class AgeRangeAttribute(int minAge, int maxAge) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateOnly dob)
            return false;

        var today = DateOnly.FromDateTime(DateTime.Now);

        return dob <= today.AddYears(-minAge)
            && dob >= today.AddYears(-maxAge);
    }
}
