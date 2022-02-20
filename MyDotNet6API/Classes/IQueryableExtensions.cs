using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyDotNet6API.Classes
{
    //-------We add this helper class to make sorting easy and dynamic----------//
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> MyCustomOrderBy<TEntity>(this IQueryable<TEntity> items, string sortBy, string sortOrder)
        {
            var type = typeof(TEntity);
            var expression2 = Expression.Parameter(type, "t");
            var property = type.GetProperty(sortBy);
            var expression1 = Expression.MakeMemberAccess(expression2, property);
            var lambda = Expression.Lambda(expression1, expression2);
            var result = Expression.Call(
                typeof(Queryable),
                sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                new Type[] { type, property.PropertyType },
                items.Expression,
                Expression.Quote(lambda));

            return items.Provider.CreateQuery<TEntity>(result);
        }

    }
}
