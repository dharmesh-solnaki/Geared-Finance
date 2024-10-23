namespace Entities.DTOs;
public class VendorDTO : BaseDTO
{
    public string Name { get; set; } = null!;
}

public class RelationshipManagerDTO : BaseDTO
{
    public string Name { get; set; } = null!;
    public string SurName { get; set; } = null!;
    public bool Status { get; set; } = true;
}

public class ManagerLevelDTO : BaseDTO
{
    public string LevelName { get; set; } = null!;
    public int LevelNo { get; set; }
}
