using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class DocumentType : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}