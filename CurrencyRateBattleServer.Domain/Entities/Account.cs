namespace CurrencyRateBattleServer.Domain.Entities;

public sealed class Account
{
    public Guid Id { get; }
    
    public decimal Amount { get; set; }
    
    public User User { get; set; }

    public static Account Create(decimal amount, User user = null!)
    {
        return new Account { Amount = amount, User = user };
    }

    public static Account Create()
    {
        return new Account();
    }
    
    public void AddStartBalance(decimal startBalance)
    {
        Amount = startBalance;
    }
}
