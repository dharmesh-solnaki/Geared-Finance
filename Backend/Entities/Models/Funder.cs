using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("Funder")]
public partial class Funder
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    public string Name { get; set; } = null!;

    [Column("ABN")]
    [StringLength(11)]
    public string Abn { get; set; } = null!;

    public bool Status { get; set; }

    [StringLength(150)]
    public string? Bank { get; set; }

    [Column("BSB")]
    [StringLength(6)]
    public string? Bsb { get; set; }

    [StringLength(17)]
    public string? Account { get; set; }

    [StringLength(150)]
    public string? StreetAddress { get; set; }

    [Column("SASuburb")]
    [StringLength(100)]
    public string? Sasuburb { get; set; }

    [Column("SAState")]
    [StringLength(100)]
    public string? Sastate { get; set; }

    [Column("SAPostcode")]
    [StringLength(6)]
    public string? Sapostcode { get; set; }

    [StringLength(150)]
    public string? PostalAddress { get; set; }

    [Column("PASuburb")]
    [StringLength(100)]
    public string? Pasuburb { get; set; }

    [Column("PAState")]
    [StringLength(100)]
    public string? Pastate { get; set; }

    [Column("PAPostcode")]
    [StringLength(6)]
    public string? Papostcode { get; set; }

    [StringLength(150)]
    public string? ApplicationEmail { get; set; }

    [StringLength(150)]
    public string? SettlementsEmail { get; set; }

    [StringLength(150)]
    public string? AdminEmail { get; set; }

    [StringLength(150)]
    public string? PayoutsEmail { get; set; }

    [StringLength(150)]
    public string? CollectionEmail { get; set; }

    [Column("EOTEmail")]
    [StringLength(150)]
    public string? Eotemail { get; set; }

    [Column("BDMName")]
    [StringLength(150)]
    public string Bdmname { get; set; } = null!;

    [Column("BDMSurname")]
    [StringLength(150)]
    public string Bdmsurname { get; set; } = null!;

    [Column("BDMEmail")]
    [StringLength(150)]
    public string Bdmemail { get; set; } = null!;

    [Column("BDMPhone")]
    [StringLength(10)]
    public string? Bdmphone { get; set; }

    [Column(TypeName = "character varying")]
    public string? LogoImg { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    [StringLength(150)]
    public string? EntityName { get; set; }

    [InverseProperty("Funder")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("Funder")]
    public virtual FunderProductGuide? FunderProductGuide { get; set; }

    [InverseProperty("Funder")]
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    [InverseProperty("Funder")]
    public virtual ICollection<RateChartOption> RateChartOptions { get; set; } = new List<RateChartOption>();
}
