using System.ComponentModel;

namespace CurrencyRateBattleServer.Helpers;
public class WebServerOptions
{
    public const string SectionName = "CBRSettings";

    [DefaultValue(1000)]
    public decimal RegistrationReward { get; set; }

    [DefaultValue(180)]
    public int DaysToStoreExchanges { get; set; }

    [DefaultValue(3)]
    public int DaysToGenerateRooms { get; set; }
}
