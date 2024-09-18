using Entities.DTOs;
using Entities.Models;

namespace Service.Extensions;
public static class NoteMapping
{
    public static NoteDTO ToDto(this Note note)
    {
        return new NoteDTO()
        {
            Id = note.Id,
            UserName = $"{note.CreatedByNavigation.Name} {note.CreatedByNavigation.SurName}",
            Description = note.Description,
            CreatedDate = new DateTimeOffset(note.CreatedDate).ToUnixTimeMilliseconds().ToString(),
        };
    }

    public static Note FromDto(this NoteDTO note)
    {
        return new Note()
        {
            Id = note.Id,
            NoteType = 1,
            Description = note.Description,
            CreatedDate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(note.CreatedDate)).DateTime,
            IsDeleted = false,
        };
    }
}
