using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Repositories
{
    public class DocumentRepositoryAsync:GenericRepositoryAsync<Document>,IDocumentRepositoryAsync
    {
        private readonly DbSet<Document> _documents;

        public DocumentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _documents = dbContext.Set<Document>();
        }
    }
}
