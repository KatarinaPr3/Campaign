using Microsoft.EntityFrameworkCore;
using CampaignAPI.DB.Interfaces;

namespace CampaignAPI.DB.Service
{
    public class EntityService<T> : IEntityService<T> where T : class
    {
        private readonly CampaignDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityService(CampaignDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with id {id} not found.");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
