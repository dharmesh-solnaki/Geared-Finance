using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs;

public class FunderDTO : BaseDTO
{
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [StringLength(11)]
    public string Abn { get; set; } = null!;

    public bool Status { get; set; }

    [StringLength(150)]
    public string? Bank { get; set; }

    [StringLength(6)]
    public string? Bsb { get; set; }

    [StringLength(17)]
    public string? Account { get; set; }

    [StringLength(150)]
    public string? StreetAddress { get; set; }

    [StringLength(100)]
    public string? Suburb { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(6)]
    public string? Postcode { get; set; }

    [StringLength(150)]
    public string? PostalAddress { get; set; }

    [StringLength(100)]
    public string? PostalSuburb { get; set; }

    [StringLength(100)]
    public string? PostalState { get; set; }

    [StringLength(6)]
    public string? PostalPostcode { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? CreditAppEmail { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? SettlementsEmail { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? AdminEmail { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? PayoutsEmail { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? CollectionEmail { get; set; }

    [StringLength(150)]
    [EmailAddress]
    public string? EotEmail { get; set; } = null;

    [StringLength(150)]
    public string BdmName { get; set; } = null!;

    [StringLength(150)]
    public string BdmSurname { get; set; } = null!;

    [StringLength(150)]
    [EmailAddress]
    public string BdmEmail { get; set; } = null!;

    [StringLength(10)]
    public string? BdmPhone { get; set; }

    public string? LogoImg { get; set; }

    public string? ImgType { get; set; }
    public string? EntityName { get; set; } = null!;
}


