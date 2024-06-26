namespace Internships.Core.Entities
{
    public class InternshipStatus : AuditableBaseEntity
    {
        public int InternshipId { get; set; }
        public virtual Internship Internship { get; set; }
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }
        public bool IsEnabled { get; set; }
        public string Comment { get; set; }
    }
}