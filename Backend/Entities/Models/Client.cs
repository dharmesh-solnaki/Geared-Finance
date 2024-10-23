using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public partial class Client
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "character varying")]
    public string? EntityName { get; set; }

    [Column("ABN", TypeName = "character varying")]
    public string? Abn { get; set; }

    [Column(TypeName = "character varying")]
    public string? TradingName { get; set; }
}
