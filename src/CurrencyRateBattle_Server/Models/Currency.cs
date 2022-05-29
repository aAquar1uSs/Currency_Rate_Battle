using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateBattleServer.Models;

[Index(nameof(CurrencyName), IsUnique = true)]
public class Currency
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(3)")]
    public string CurrencyName { get; set; } = default!;

    [Column(TypeName = "varchar(3)")]
    public string CurrencySymbol { get; set; } = default!;

    [Required]
    [Column(TypeName = "varchar(128)")]
    public string Description { get; set; } = default!;

}
