using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Document
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string FileName { get; set; } = null!;

    public int FunderId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("FunderId")]
    [InverseProperty("Documents")]
    public virtual Funder Funder { get; set; } = null!;
}
