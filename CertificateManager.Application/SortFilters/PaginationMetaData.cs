namespace CertificateManager.Application.SortFilters;

public class PaginationMetaData
{
    public int TotalListCount { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPageNumber { get; set; }
    public bool HasNextPage => CurrentPageNumber < TotalPage;
    public bool HasPreviousPage => CurrentPageNumber > 1;

    public PaginationMetaData(int totalListCount, int pageSize = 1, int currentPageNumber = 1)
    {
        TotalListCount = totalListCount;
        PageSize = pageSize;
        CurrentPageNumber = currentPageNumber;
        TotalPage = (int)Math.Ceiling(totalListCount / (double)pageSize);
    }
}