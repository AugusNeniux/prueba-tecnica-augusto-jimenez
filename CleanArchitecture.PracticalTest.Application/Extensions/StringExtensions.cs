using CleanArchitecture.PracticalTest.Application.Contracts.ContextApplication;

namespace CleanArchitecture.PracticalTest.Application.Extensions;

public static class StringExtensions
{
    public static string ToLocalizedEntityName(this string entityType, ILocalizer localizer)
    {
        return localizer.GetDomainConcept(entityType);
    }

    public static string ToLocalizedEnumValue(this string enumValue, ILocalizer localizer)
    {
        return localizer.GetEnumValue(enumValue);
    }
}
