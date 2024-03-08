namespace Certificate.Application.SortFilters;

public class PaginationParams
{
    private const int MaxAmountData = 500;
    private const int MinPageNumber = 1;

    private int _amountData = 5;
    private int _pageNumber = 1;

    public int PageSize
    {
        get => _amountData > MaxAmountData ? MaxAmountData : _amountData <= 0 ? 1 : _amountData;
        set => _amountData = value;
    }

    public int Page
    {
        get => _pageNumber < MinPageNumber ? MinPageNumber : _pageNumber;
        set => _pageNumber = value;
    }
}