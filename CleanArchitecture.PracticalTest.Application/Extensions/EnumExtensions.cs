using System.Text.RegularExpressions;
using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;

namespace CleanArchitecture.PracticalTest.Application.Extensions;

public static class EnumExtensions
{
    public static string ToLocalizedString<TEnum>(this TEnum enumValue, ILocalizer localizer)
        where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var enumName = Enum.GetName(enumType, enumValue);

        if (enumName == null)
            return enumValue.ToString();

        var key = $"{enumType.Name}.{enumName}";
        return localizer.GetEnumValue(key) ?? enumValue.ToString();
    }

    public static string ToKebabCase(this Enum value)
    {
        var name = value.ToString();
        // Inserta guión antes de cada mayúscula (excepto la primera) y pasa a minúsculas
        return Regex.Replace(name, "(?<!^)([A-Z])", "-$1").ToLower();
    }

}
