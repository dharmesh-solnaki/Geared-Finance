using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("AppSource")]
public partial class AppSource
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string Name { get; set; } = null!;

    public short SortOrder { get; set; }

    [InverseProperty("Appsource")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}
