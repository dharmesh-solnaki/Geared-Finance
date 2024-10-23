namespace Entities.DTOs;
public class SelectedFundingDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public SubCategory[] SubCategory { get; set; }

}

public class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
