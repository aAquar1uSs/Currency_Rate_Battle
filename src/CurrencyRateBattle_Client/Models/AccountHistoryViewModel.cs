using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRBClient.Models;

public class AccountHistoryViewModel
{
    public Guid AccountHistoryId { get; set; }

    public Guid? RoomId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    //IsCredit = true for credit transactions; false - for debit transactions
    public bool IsCredit { get; set; }
}
