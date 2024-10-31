using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KoiCare.Application.Helpers
{
    public static class EnumExtensions
    {
        public static string? GetDisplayName(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null)
            {
                return enumValue.ToString();
            }

            var displayAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
            return displayAttribute?.Name ?? enumValue.ToString();
        }
    }
}
