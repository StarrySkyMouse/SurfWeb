using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Core.Utils.Extensions
{
    public static class LinqExtension
    {
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (!condition)
            {
                return query;
            }
            return query.Where(predicate);
        }
        public static IQueryable<T> PageData<T>([NotNull] this IQueryable<T> query, int pageIndex, int pageSize)
        {
            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
        public static IEnumerable<T> PageData<T>([NotNull] this IEnumerable<T> query, int pageIndex, int pageSize)
        {
            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
