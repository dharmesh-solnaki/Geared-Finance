namespace Entities.DTOs;

public class NoteDTO
{
    public int Id { get; set; }
    public string CreatedDate { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Description { get; set; } = null!;
}
