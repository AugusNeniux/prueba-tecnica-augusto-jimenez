// using System;

// namespace CleanArchitecture.PracticalTest.Application.Extensions;

// public static class QueryExtensions
// {
//     private readonly static ParsingConfig config = new()
//     {
//         IsCaseSensitive = false
//     };

//     public static IQueryable<T> ApplyPeriod<T>(
//         this IQueryable<T> query,
//         PagingParams pagingParams
//     )
//     {
//         if (!string.IsNullOrWhiteSpace(pagingParams.PeriodField)
//             && pagingParams.Period?.IsValid == true)
//         {
//             var startDate = pagingParams.Period.Start!.Value;
//             var endDate = pagingParams.Period.End!.Value;

//             var expression = $"{pagingParams.PeriodField} >= @0 && {pagingParams.PeriodField} <= @1";
//             query = query.Where(expression, startDate, endDate);
//         }

//         return query;
//     }

//     public static IQueryable<T> ApplySpec<T>(
//         this IQueryable<T> query,
//         PagingParams pagingParams,
//         HashSet<string>? allowedFilters = null)
//     {
//         // Filtros cerrados
//         if (pagingParams.Filters is not null)
//         {
//             foreach (var filter in pagingParams.Filters)
//             {
//                 if (filter.Selected is null || filter.Selected.Count == 0 || string.IsNullOrWhiteSpace(filter.Field))
//                     continue;

//                 var field = filter.Field;
//                 var values = CastFilterValuesByType(filter.Type, filter.Selected);

//                 if (values.Count == 0) continue;
//                 if (allowedFilters != null && !allowedFilters.Contains(field)) continue;

//                 var expression = $"@0.Contains({field})";
//                 query = query.Where(config, expression, values);
//             }
//         }

//         // Ordenamiento
//         if (!string.IsNullOrWhiteSpace(pagingParams.SortBy))
//         {
//             var orderBy = $"{pagingParams.SortBy} {(pagingParams.IsSortAscending ? "asc" : "desc")}";
//             query = query.OrderBy(orderBy);
//         }

//         return query;
//     }

//     public static IQueryable<T> ApplyPagination<T>(
//         this IQueryable<T> query,
//         PagingParams pagingParams)
//     {
//         // Paginación
//         return query
//             .Skip((pagingParams.PageIndex - 1) * pagingParams.PageSize)
//             .Take(pagingParams.PageSize);
//     }



//     private static IList CastFilterValuesByType(string type, IEnumerable<object> values)
//     {
//         var elements = values.Cast<JsonElement>();

//         return type.ToLowerInvariant() switch
//         {
//             "int32" => elements.Select(e => e.GetInt32()).ToList(),
//             "int64" => elements.Select(e => e.GetInt64()).ToList(),
//             "boolean" => elements.Select(e => e.GetBoolean()).ToList(),
//             "string" => elements.Select(e => e.GetString()).ToList(),
//             "guid" => elements.Select(e => Guid.Parse(e.GetString()!)).ToList(),
//             "datetime" => elements.Select(e => e.GetDateTime()).ToList(),
//             _ => throw new NotSupportedException($"Type '{type}' is not supported.")
//         };
//     }
// }
