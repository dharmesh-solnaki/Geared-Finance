using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs;

public class FunderGuideTypeDTO : BaseDTO
{
    [StringLength(25)]
    public string? FinanceType { get; set; }

    [StringLength(150)]
    public string? Rates { get; set; }

    public bool IsBrokerageCapped { get; set; }
    public bool IsApplyRitcFee { get; set; }
    public string? RitcFee { get; set; }
    public bool IsApplyAccountKeepingFee { get; set; }
    public string? AccountKeepingFee { get; set; }
    public bool IsApplyDocumentFee { get; set; }
    public string? FunderDocFee { get; set; }
    public string? MatrixNotes { get; set; }
    public string? GeneralNotes { get; set; }

    [StringLength(150)]
    public string? Cutoff { get; set; }

    [StringLength(150)]
    public string? Craa { get; set; }

    public string? EotNotes { get; set; }

    public int? FunderId { get; set; }

    public SelectedFundingDTO[]? SelectedFundings { get; set; } = null!;
}
