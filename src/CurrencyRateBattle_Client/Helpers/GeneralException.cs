using System.Globalization;

namespace CRBClient.Helpers;

public class GeneralException : Exception
{
    public GeneralException() : base() { }

    public GeneralException(string message) : base(message) { }

    public GeneralException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {

    }
}
