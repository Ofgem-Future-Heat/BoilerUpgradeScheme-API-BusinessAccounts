using Microsoft.EntityFrameworkCore;
using Ofgem.API.BUS.BusinessAccounts.Domain.Request;

namespace Ofgem.API.BUS.BusinessAccounts.Providers.DataAccess.Helpers;

public static class Pagination
{
    /// <summary>
    /// GetPagedAsync - Pagination of data into page size chunks at the page designated.
    /// </summary>
    /// <typeparam name="T">The data object to be used.</typeparam>
    /// <param name="query">The sql to query the data.</param>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Page size - number of items per page.</param>
    /// <returns>IList of data objects.</returns>
    public static async Task<PagedResult<T>> GetPagedAsync<T>(this IQueryable<T> query,
        int page, int pageSize) where T : class
    {
        var result = new PagedResult<T>
        {
            CurrentPage = page,
            PageSize = pageSize,
            RowCount = await query.CountAsync()
        };

        var pageCount = (double)result.RowCount / pageSize;
        result.PageCount = (int)Math.Ceiling(pageCount);

        var skip = (page - 1) * pageSize;
        result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

        return result;
    }
}