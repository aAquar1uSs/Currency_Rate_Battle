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
        return (CurrencyName is not null && !string.IsNullOrWhiteSpace(CurrencyName))
            || (StartDate is not null && !string.IsNullOrWhiteSpace(StartDate))
            || (EndDate is not null && !string.IsNullOrWhiteSpace(EndDate));
    }
}
