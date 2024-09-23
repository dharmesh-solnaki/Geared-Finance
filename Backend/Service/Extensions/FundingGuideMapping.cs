using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions
{
    public static class FundingGuideMapping
    {
        public static FunderGuideTypeDTO ToDto(this FunderProductGuide funderProductGuide)
        {
            return new FunderGuideTypeDTO
            {
                Id = funderProductGuide.Id,
                FinanceType = funderProductGuide.TypeOfFinance,
                Rates = funderProductGuide.Rates,
                IsBrokerageCapped = funderProductGuide.IsBrokerageCapped,
                IsApplyRitcFee = funderProductGuide.IsApplyRitcfee,
                RitcFee = funderProductGuide.Ritcfee?.ToString("F2"),
                IsApplyAccountKeepingFee = funderProductGuide.IsApplyAccountKeepingFee,
                AccountKeepingFee = funderProductGuide.AccountKeepingFee?.ToString("F2"),
                IsApplyDocumentFee = funderProductGuide.IsApplyDocumentFee,
                FunderDocFee = funderProductGuide.FunderDocFee?.ToString("F2"),
                MatrixNotes = funderProductGuide.MatrixNotes,
                GeneralNotes = funderProductGuide.GeneralNotes,
                Cutoff = funderProductGuide.CutOff,
                Craa = funderProductGuide.Craa,
                EotNotes = funderProductGuide.Eotnotes,
                FunderId = funderProductGuide.FunderId,
            };
        }
        public static FunderProductGuide FromDto(this FunderGuideTypeDTO funderGuideTypeDTO)
        {
            return new FunderProductGuide
            {
                Id = funderGuideTypeDTO.Id == null ? 0 : (int)funderGuideTypeDTO.Id,
                TypeOfFinance = funderGuideTypeDTO.FinanceType,
                Rates = funderGuideTypeDTO.Rates,
                IsBrokerageCapped = funderGuideTypeDTO.IsBrokerageCapped,
                IsApplyRitcfee = funderGuideTypeDTO.IsApplyRitcFee,
                Ritcfee = string.IsNullOrEmpty(funderGuideTypeDTO.RitcFee) ? (decimal?)null : decimal.Parse(funderGuideTypeDTO.RitcFee),
                IsApplyAccountKeepingFee = funderGuideTypeDTO.IsApplyAccountKeepingFee,
                AccountKeepingFee = string.IsNullOrEmpty(funderGuideTypeDTO.AccountKeepingFee) ? (decimal?)null : decimal.Parse(funderGuideTypeDTO.AccountKeepingFee),
                IsApplyDocumentFee = funderGuideTypeDTO.IsApplyDocumentFee,
                FunderDocFee = string.IsNullOrEmpty(funderGuideTypeDTO.FunderDocFee) ? (decimal?)null : decimal.Parse(funderGuideTypeDTO.FunderDocFee),
                MatrixNotes = funderGuideTypeDTO.MatrixNotes,
                GeneralNotes = funderGuideTypeDTO.GeneralNotes,
                CutOff = funderGuideTypeDTO.Cutoff,
                Craa = funderGuideTypeDTO.Craa,
                Eotnotes = funderGuideTypeDTO.EotNotes,
                FunderId = (int)funderGuideTypeDTO.FunderId,
                IsDeleted = false
            };
        }
    }
}
