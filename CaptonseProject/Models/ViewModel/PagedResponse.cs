public class PagedResponse<T>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; } = 0;
    public int TotalRecords { get; set; } = 0;
    public T? Data { get; set; }
    
    // public bool HasPreviousPage => PageNumber > 1;
    // public bool HasNextPage => PageNumber < TotalPages;
}