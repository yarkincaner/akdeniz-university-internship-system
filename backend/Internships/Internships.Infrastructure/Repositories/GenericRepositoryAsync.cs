using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internships.Core.Interfaces;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Internships.Infrastructure.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IQueryable<T>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _dbContext.Set<T>()
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public void AttachModified(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void AttachAdded(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
        }
    }
}