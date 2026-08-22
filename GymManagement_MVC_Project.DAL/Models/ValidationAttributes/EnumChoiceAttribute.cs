namespace GymManagement_MVC_Project.DAL.Models.ValidationAttributes;

using System.ComponentModel.DataAnnotations;

public class EnumChoiceAttribute<TEnum> : ValidationAttribute
    where TEnum : struct, Enum
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        return value is TEnum enumValue && Enum.IsDefined(typeof(TEnum), enumValue);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} contains an invalid value.";
    }
}
