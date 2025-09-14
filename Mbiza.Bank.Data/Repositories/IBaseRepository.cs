using System.Linq.Expressions;

namespace Mbiza.Bank
{
    public interface IBaseRepository<TEntity>
    {
        Task CreateAsync(TEntity entity);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task<TEntity> GetByQueryAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
