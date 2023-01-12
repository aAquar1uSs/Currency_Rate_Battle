using System.Globalization;

namespace CurrencyRateBattleServer.Domain.Infrastructure;

public class Filter
{
    public string CurrencyName { get; set; }
    
    public string StartDate { get; set; }
    
    public string EndDate { get; set; }

    public const string DateFormat = "MM.dd.yyyy HH";
    
    private static readonly TimeSpan _timeDifference = DateTime.UtcNow - DateTime.Now;
    
    public Filter(string currencyName, string startDate, string endDate)
    {
        CurrencyName = currencyName;
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool DateTryParse(string date, out DateTime dateTime)
    {
        try
        {
            dateTime = DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture)
                       + _timeDifference;
            return true;
        }
        catch
        {
            dateTime = default;
            return false;
        }
    }
}
