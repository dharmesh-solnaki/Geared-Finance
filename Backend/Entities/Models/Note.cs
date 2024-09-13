using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Note
{
    [Key]
    public int Id { get; set; }

    public short NoteType { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    public int? FunderId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("FunderId")]
    [InverseProperty("Notes")]
    public virtual Funder? Funder { get; set; }
}
