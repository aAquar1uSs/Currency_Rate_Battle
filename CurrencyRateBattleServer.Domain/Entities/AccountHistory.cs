namespace CurrencyRateBattleServer.Domain.Entities;

public class AccountHistory
{
    public Guid Id { get; }
    
    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    //IsCredit = true for credit transactions; dalse - for debit transactions
    public bool IsCredit { get; set; }

    public Room? Room { get; set; }
    
    public Account Account { get; set; } = default!;
}
