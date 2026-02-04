using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.PracticalTest.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var stringBuilder = new System.Text.StringBuilder();
        var previousUpper = false;

        foreach (char c in input)
        {
            if (char.IsUpper(c))
            {
                if (stringBuilder.Length > 0 && !previousUpper)
                    stringBuilder.Append('_');

                stringBuilder.Append(char.ToLower(c));
                previousUpper = true;
            }
            else
            {
                stringBuilder.Append(c);
                previousUpper = false;
            }
        }

        return stringBuilder.ToString();
    }

    public static void ToSnakeCaseTable<TEntity>(this EntityTypeBuilder<TEntity> builder, string schema)
        where TEntity : class
    {
        var entityName = typeof(TEntity).Name;
        builder.ToTable(entityName.ToSnakeCase(), schema);
    }
}
