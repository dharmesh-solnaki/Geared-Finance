using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models;

public partial class Document
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string FileName { get; set; } = null!;

    [Column(TypeName = "character varying")]
    public string FilePath { get; set; } = null!;

    public int FunderId { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("FunderId")]
    [InverseProperty("Documents")]
    public virtual Funder Funder { get; set; } = null!;
}
