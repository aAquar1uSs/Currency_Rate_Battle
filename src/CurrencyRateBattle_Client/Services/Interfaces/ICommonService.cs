using CRBClient.Models;

namespace CRBClient.Services.Interfaces;
public interface ICommonService
{
    Task<string> GetUserBalanceAsync();
}
