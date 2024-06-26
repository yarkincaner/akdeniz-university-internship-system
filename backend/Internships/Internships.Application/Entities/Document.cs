namespace Internships.Core.Entities
{
    public class Document : AuditableBaseEntity
    {
        public int InternshipId { get; set; }
        public Internship Internship { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }

        public string DocumentURL { get; set; }
    }
}