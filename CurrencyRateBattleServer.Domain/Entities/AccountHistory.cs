namespace CurrencyRateBattleServer.Domain.Entities;

public class AccountHistory
{
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public bool IsCredit { get; set; }

    public Room? Room { get; set; }

    public Account Account { get; set; }

    public static AccountHistory Create(Guid accountId, DateTime date, decimal amount, bool isCredit = false)
    {
        return new() { IsCredit = isCredit, Date = date, Amount = amount, Id = accountId };
    }

    public void AddRoom(Room room)
    {
        Room = room;
    }

    public void AddAccount(Account account)
    {
        Account = account;
    }
    
}
