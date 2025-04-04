using System.Linq.Expressions;

namespace MunicipalityTaxesAPI.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        //Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        //void Remove(TEntity entity);
        //void Update(TEntity entity);
        Task SaveChangesAsync();
        //Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        //Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> expression);
    }
}
