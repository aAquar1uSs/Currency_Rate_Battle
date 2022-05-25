using System.Globalization;
using System.Text.Json.Serialization;

namespace CurrencyRateBattleServer.Models;

public class Filter
{
    [JsonPropertyName("currencyName")]
    public string CurrencyName { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }

    [JsonIgnore]
    public const string DateFormat = "MM.dd.yyyy HH";

    [JsonIgnore]
    private static readonly TimeSpan _timeDifference = DateTime.UtcNow - DateTime.Now;

    [JsonConstructor]
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
