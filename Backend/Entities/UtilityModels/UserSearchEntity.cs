namespace Entities.UtilityModels
{
    public class UserSearchEntity :BaseModelSearchEntity
    { 
        public string? name { get; set; }
        public string? roleName { get; set; }
    }
}
