using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DNTFrameworkCore.Querying;
using NHibernate.Linq;

namespace DNTFrameworkCore.NHibernate.Linq
{
    public static class QueryableExtensions
    {
        public static Task<IPagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query,
            IPagedRequest request, CancellationToken cancellationToken = default)
        {
            var filtering = request is IFilteredPagedRequest q ? q.Filtering : null;

            return query.ToPagedListAsync(request.Page, request.PageSize, request.SortExpression, filtering,
                cancellationToken);
        }

        public static Task<IPagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query,
            int page,
            int pageSize,
            string sortExpression,
            CancellationToken cancellationToken = default)
        {
            return query.ToPagedListAsync(page, pageSize, sortExpression, null, cancellationToken);
        }

        public static async Task<IPagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query,
            int page,
            int pageSize,
            string sortExpression,
            FilteringCriteria filtering,
            CancellationToken cancellationToken = default)
        {
            query = query.ApplyFiltering(filtering);

            var totalCount = await query.LongCountAsync(cancellationToken);

            query = query.ApplySorting(sortExpression);
            query = query.ApplyPaging(page, pageSize);

            return new PagedResult<T>
            {
                ItemList = await query.ToListAsync(cancellationToken),
                TotalCount = totalCount
            };
        }
    }
}