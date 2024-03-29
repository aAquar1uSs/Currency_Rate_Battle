﻿
namespace CurrencyRateBattleServer.ApplicationServices.Dto;

public class AccountHistoryDto
{
    public Guid AccountHistoryId { get; set; }
    
    public Guid? RoomId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    //IsCredit = true for credit transactions; false - for debit transactions
    public bool IsCredit { get; set; }
}
