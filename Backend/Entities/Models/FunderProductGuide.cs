using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

[Table("FunderProductGuide")]
[Index("FunderId", Name = "UQ_FunderProductGuide_FunderId", IsUnique = true)]
public partial class FunderProductGuide
{
    [Key]
    public int Id { get; set; }

    [StringLength(25)]
    public string? TypeOfFinance { get; set; }

    [StringLength(150)]
    public string? Rates { get; set; }

    public bool IsBrokerageCapped { get; set; }

    [Column("IsApplyRITCFee")]
    public bool IsApplyRitcfee { get; set; }

    [Column("RITCFee")]
    [Precision(15, 2)]
    public decimal? Ritcfee { get; set; }

    public bool IsApplyAccountKeepingFee { get; set; }

    [Precision(15, 2)]
    public decimal? AccountKeepingFee { get; set; }

    public bool IsApplyDocumentFee { get; set; }

    [Precision(15, 2)]
    public decimal? FunderDocFee { get; set; }

    [Column(TypeName = "character varying")]
    public string? MatrixNotes { get; set; }

    [Column(TypeName = "character varying")]
    public string? GeneralNotes { get; set; }

    [StringLength(150)]
    public string? CutOff { get; set; }

    [Column("CRAA")]
    [StringLength(150)]
    public string? Craa { get; set; }

    [Column("EOTNotes", TypeName = "character varying")]
    public string? Eotnotes { get; set; }

    public int FunderId { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("FunderId")]
    [InverseProperty("FunderProductGuide")]
    public virtual Funder Funder { get; set; } = null!;

    [InverseProperty("FundingProductGuide")]
    public virtual ICollection<FunderProductFunding> FunderProductFundings { get; set; } = new List<FunderProductFunding>();
}
