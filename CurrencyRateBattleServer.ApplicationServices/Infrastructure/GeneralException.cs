using System.Globalization;

namespace CurrencyRateBattleServer.ApplicationServices.Infrastructure;
public class GeneralException : Exception
{
    public GeneralException() { }

    public GeneralException(string message) : base(message) { }

    public GeneralException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
