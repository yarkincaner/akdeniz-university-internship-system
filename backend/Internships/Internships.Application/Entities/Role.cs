namespace Internships.Core.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public UserRoles UserRoles { get; set; }
    }
}