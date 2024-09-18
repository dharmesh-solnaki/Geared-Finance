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
            id = doc.Id,
            FileName = doc.FileName,
            CreatedDate = doc.CreatedDate.ToString("dd/MM/yyyy", new CultureInfo("en-US"))
        };
    }
}
