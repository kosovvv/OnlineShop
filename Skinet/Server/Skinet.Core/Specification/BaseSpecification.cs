using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            Includes = new List<Expression<Func<T, object>>>();
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria) : this() 
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public ICollection<Expression<Func<T, object>>> Includes { get; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        { 
            Includes.Add(includeExpression); 
        }
    }
}
