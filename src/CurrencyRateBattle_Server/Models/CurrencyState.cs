using System.ComponentModel.DataAnnotations;
namespace CurrencyRateBattleServer.Models;

public class CurrencyState
{
    [Key]
    public int Id { get; }

    [Required]
    public DateTime Date { get; set; }

    public decimal? USDValue { get; set; }
    public decimal? EURValue { get; set; }
    public decimal? PLNValue { get; set; }
    public decimal? GBPValue { get; set; }
    public decimal? CHFValue { get; set; }
}
