using Microsoft.EntityFrameworkCore;
using CleanArchitecture.PracticalTest.Application.Contracts.Data;
using CleanArchitecture.PracticalTest.Domain.Common;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data.Specification;

public class SpecificationEvaluator<T> where T : BaseDomainModel
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        var query = inputQuery;

        // Modifica el IQueryable utilizando la expresión de criterios de la especificación
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // Agrega el ordenamiento basado en la especificación
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        // Agrega el paginado basado en la especificación
        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        // Agrega todos los includes basados en la especificación
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
