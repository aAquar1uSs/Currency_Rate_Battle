using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CRBClient.Helpers;
public class WebServerOptions
{
    public const string SectionName = "WebServer";
    [Required]
    [Url]
    public string? BaseUrl { get; set; }

    public string? RoomsURL { get; set; }

    [Required]
    [DefaultValue("get-balance")]
    public string? GetBalanceURL { get; set; }

    public string? FilterURL { get; set; }

    public string? GetUserBetsURL { get; set; }

    public string? GetUsersRatingURL { get; set; }

    public string? RegistrationAccURL { get; set; }

    public string? LoginAccURL { get; set; }

    public string? UserProfileURL { get; set; }

    public string? AccountHistoryURL { get; set; }

    public string? MakeBetURL { get; set; }

    public string? GetCurrencyRatesURL { get; set; }
}
