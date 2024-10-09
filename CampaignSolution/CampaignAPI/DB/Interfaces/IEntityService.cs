using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CampaignAPI.DB.Interfaces
{
    public interface IEntityService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        
    }
}
