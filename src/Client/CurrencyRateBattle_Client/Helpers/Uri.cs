namespace CRBClient.Helpers;

public class Uri
{
    public const string SectionName = "Uris";

    public string RoomsURL { get; set; } 
    
    public string GetBalanceURL { get; set; }

    public string RoomsFilterURL { get; set; }

    public string GetUserBetsURL { get; set; }

    public string GetUsersRatingURL { get; set; }

    public string RegistrationAccURL { get; set; }

    public string LoginAccURL { get; set; }

    public string UserProfileURL { get; set; }

    public string AccountHistoryURL { get; set; }

    public string MakeBetURL { get; set; }

    public string GetCurrencyRatesURL { get; set; }
}
