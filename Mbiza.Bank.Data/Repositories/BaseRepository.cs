using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mbiza.Bank
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        #region Properties

        protected MbizaContext MbizaContext { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="mbizaContext"></param>
        protected BaseRepository(MbizaContext archContext) => MbizaContext = archContext;

        #endregion

        #region Implemented Members

        /// <summary>
        /// Creates the entity asynchronously
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task CreateAsync(TEntity entity) => await MbizaContext.Set<TEntity>().AddAsync(entity);

        /// <summary>
        /// Updates the entity asynchronously
        /// </summary>MO
        /// <param name="entity"></param>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            MbizaContext.Set<TEntity>().Update(entity);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Deletes the entity asynchronously
        /// </summary>
        /// <param name="entity"></param>
        public async Task<bool> DeleteAsync(TEntity entity)
        {
            MbizaContext.Set<TEntity>().Remove(entity);
            return await Task.FromResult(true);
        }

        /// <summary>
        /// Gets all entities asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await MbizaContext.Set<TEntity>().AsNoTracking().ToArrayAsync();

        /// <summary>
        /// Gets all entities by query or filter asynchronously
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByQueryAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await MbizaContext.Set<TEntity>().Where(expression).AsNoTracking().FirstOrDefaultAsync();
        }

        #endregion
    }
}
