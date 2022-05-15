using System.Security.Cryptography;
using System.Text;

namespace CurrencyRateBattleServer.Tools;

public class Sha256Encoder : IEncoder
{
    public string Encrypt(string password)
    {
        using var sha256 = SHA256.Create();
        var sourceByte = Encoding.UTF8.GetBytes(password);
        var hashValue = sha256.ComputeHash(sourceByte);

        return BitConverter.ToString(hashValue);
    }
}
