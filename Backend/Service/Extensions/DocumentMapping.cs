using Entities.DTOs;
using Entities.Models;
using System.Globalization;

namespace Service.Extensions;

public static class DocumentMapping
{
    public static DocumentDTO ToDto(this Document doc)
    {
        return new DocumentDTO()
        {
            Id = doc.Id,
            FileName = doc.FileName.Replace($"Funder{doc.FunderId}", string.Empty),
            CreatedDate = doc.CreatedDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"))
        };
    }
}
