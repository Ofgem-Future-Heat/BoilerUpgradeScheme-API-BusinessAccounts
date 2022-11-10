namespace Ofgem.API.BUS.BusinessAccounts.Domain.Concrete;

/// <summary>
/// PagedResultBase - concrete base entity.
/// </summary>
public abstract class PagedResultBase
{
    // The current page.
    public int CurrentPage { get; set; }
    // The page count.
    public int PageCount { get; set; }
    // The page size.
    public int PageSize { get; set; }
    // The row count.
    public int RowCount { get; set; }
    // The first row in the list.
    public int FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;
    // The last row in the list.
    public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
}