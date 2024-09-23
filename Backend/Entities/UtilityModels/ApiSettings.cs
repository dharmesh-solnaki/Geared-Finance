namespace Entities.UtilityModels;

public class ApiSettings
{
    public string Secret { get; set; } = null!;
    public int AccessTokenExpTime { get; set; }
    public int RefreshTokenExpTime { get; set; }
    public int RefreshTokenExpTimeOnRemember { get; set; }
}
