using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Specification;

namespace Skinet.Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
     
            foreach (var include in spec.Includes)
            {
                query = query.Include(include);
            }
            //query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
