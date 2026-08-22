using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GymManagement_MVC_Project.BLL.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            if (value == null) return string.Empty;

            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length == 0) return value.ToString();

            var displayAttribute = memberInfo[0]
                .GetCustomAttribute<DisplayAttribute>();

            // Return the Name from the attribute, or fallback to the enum name if missing
            return displayAttribute?.GetName() ?? value.ToString();
        }
    }
}
