namespace Internships.Core.Entities
{
    public class UserRoles : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}