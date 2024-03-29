﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyRateBattleServer.Dal.Entities;

public class CurrencyDal
{
    [Key]
    [Column(TypeName = "varchar(3)")]
    public string CurrencyName { get; set; }

    [Column(TypeName = "varchar(3)")]
    public string CurrencyCode { get; set; }

    [Column(TypeName = "decimal")]
    public decimal Rate { get; set; }

    [Column(TypeName = "varchar(128)")]
    public string? Description { get; set; }

    public DateTime UpdateDate { get; set; }
}
