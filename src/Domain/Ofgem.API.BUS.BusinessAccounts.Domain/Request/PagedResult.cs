using Ofgem.API.BUS.BusinessAccounts.Domain.Concrete;

namespace Ofgem.API.BUS.BusinessAccounts.Domain.Request;

/// <summary>
/// Page results entity list.
/// </summary>
/// <typeparam name="T">Object being passed into method e.g. Business Account.</typeparam>
public class PagedResult<T> : PagedResultBase where T : class
{
    // Pagination results returned in an IList.
    public IList<T> Results { get; set; }

    /// <summary>
    /// Paged results constructor used to get/set IList results.
    /// </summary>
    public PagedResult() => Results = new List<T>();
}