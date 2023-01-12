namespace CRBClient.Dto;

public class FilterDto
{
    public string? CurrencyName { get; set; }

    public string StartDate { get; set; }

    public string EndDate { get; set; }

    public FilterDto(string currencyName, string startDate, string endDate)
    {
        CurrencyName = currencyName?.ToUpperInvariant();
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool CheckFilter()
    {
        return (!string.IsNullOrWhiteSpace(CurrencyName))
            || (!string.IsNullOrWhiteSpace(StartDate))
            || (!string.IsNullOrWhiteSpace(EndDate));
    }
}
