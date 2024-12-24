namespace Weatheryzer.Shared.Paginations;

public class PaginatedResult<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public IEnumerable<T> Items { get; set; }

    public PaginatedResult(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }
}