namespace Entities.DTOs;

public class FundingCategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class EquipmentTypeDTO
{
    public int? Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
}

public class EquipmentRepsonseDTO
{
    public int? Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsBeingUsed { get; set; }
    public FundingCategoryDTO Category { get; set; } = null!;
}
