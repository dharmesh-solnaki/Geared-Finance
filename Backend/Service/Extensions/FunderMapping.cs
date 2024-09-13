using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions;
public static class FunderMapping
{
    public static FunderDTO ToDto(this Funder funder)
    {
        return new FunderDTO()
        {
            id = funder.Id,
            Name = funder.Name,
            Abn = funder.Abn,
            Status = funder.Status,
            Bank = funder.Bank,
            Bsb = funder.Bsb,
            Account = funder.Account,
            StreetAddress = funder.StreetAddress,
            Suburb = funder.Sasuburb,
            State = funder.Sastate,
            Postcode = funder.Sapostcode,
            PostalAddress = funder.PostalAddress,
            PostalSuburb = funder.Pasuburb,
            PostalState = funder.Pastate,
            PostalPostcode = funder.Papostcode,
            CreditAppEmail = funder.ApplicationEmail,
            SettlementsEmail = funder.SettlementsEmail,
            AdminEmail = funder.AdminEmail,
            PayoutsEmail = funder.PayoutsEmail,
            CollectionEmail = funder.CollectionEmail,
            EotEmail = funder.Eotemail,
            BdmName = funder.Bdmname,
            BdmSurname = funder.Bdmsurname,
            BdmEmail = funder.Bdmemail,
            BdmPhone = funder.Bdmphone,
            LogoImg = funder.LogoImg,
            EntityName = funder.EntityName ?? funder.Name
        };
    }

    public static Funder FromDto(this FunderDTO dto)
    {
        return new Funder()
        {
            Id = dto.id == null ? 0 : (int)dto.id,
            Name = dto.Name,
            Abn = dto.Abn,
            Status = dto.Status,
            Bank = dto.Bank,
            Bsb = dto.Bsb,
            Account = dto.Account,
            StreetAddress = dto.StreetAddress,
            Sasuburb = dto.Suburb,
            Sastate = dto.State,
            Sapostcode = dto.Postcode,
            PostalAddress = dto.PostalAddress,
            Pasuburb = dto.PostalSuburb,
            Pastate = dto.PostalState,
            Papostcode = dto.PostalPostcode,
            ApplicationEmail = dto.CreditAppEmail,
            SettlementsEmail = dto.SettlementsEmail,
            AdminEmail = dto.AdminEmail,
            PayoutsEmail = dto.PayoutsEmail,
            CollectionEmail = dto.CollectionEmail,
            Eotemail = dto.EotEmail,
            Bdmname = dto.BdmName,
            Bdmsurname = dto.BdmSurname,
            Bdmemail = dto.BdmEmail,
            Bdmphone = dto.BdmPhone,
            LogoImg = dto.LogoImg,
            CreatedDate = DateTime.Now,
            CreatedBy = 0,
            IsDeleted = false,
            EntityName = dto.EntityName ?? dto.Name,
        };
    }

    public static IEnumerable<FunderDTO> ToDtoList(this IEnumerable<Funder> funders)
    {
        return funders.Select(funder => funder.ToDto());
    }

    public static DisplayFunderDTO ToDisplayFunderDTO(this Funder funder)
    {
        return new DisplayFunderDTO()
        {
            id = funder.Id,
            Funder = funder.EntityName??funder.Name,
            LegalName = funder.Name,
            FinanceType = funder.FunderProductGuide != null ? funder.FunderProductGuide.TypeOfFinance : "",
            BdmName = $"{funder.Bdmname} {funder.Bdmsurname}",
            BdmEmail = funder.Bdmemail,
            BdmPhone = funder.Bdmphone ?? "",
            Status = funder.Status
        };
    }

}
