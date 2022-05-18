using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Models;

public class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(3)")]
    public string CurrencyName { get; set; }

    [Column(TypeName = "varchar(3)")]
    public string CurrencySymbol { get; set; }

    [Required]
    [Column(TypeName = "varchar(128)")]
    public string Description { get; set; }

}
