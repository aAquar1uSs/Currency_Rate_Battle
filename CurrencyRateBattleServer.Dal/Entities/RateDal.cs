using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;
public sealed class RateDal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    //Date when the rate is added by user
    public DateTime SetDate { get; set; }

    public decimal RateCurrencyExchange { get; set; }

    public decimal Amount { get; set; }

    //Date when the rate is settled
    public DateTime? SettleDate { get; set; }

    public decimal? Payout { get; set; }

    /// <summary>
    /// IsClosed must be filled when DateTime is passed (with hosted service)
    /// </summary>
    public bool IsClosed { get; set; }

    /// <summary>
    /// IsWon is true when the rate is winning; false - for losing rate
    /// </summary>
    public bool IsWon { get; set; }

    [ForeignKey("RoomId")]
    public Guid RoomId { get; set; }

    public RoomDal Room { get; set; }
    
    public Guid CurrencyId { get; set; }

    [ForeignKey("CurrencyId")]
    public CurrencyDal Currency { get; set; }
    
    public Guid AccountId { get; set; }

    [ForeignKey("AccountId")]
    public AccountDal Account { get; set; }
}
