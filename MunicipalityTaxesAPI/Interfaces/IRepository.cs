using System.Linq.Expressions;

namespace MunicipalityTaxesAPI.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(IBaseGetRequest baseGetRequest, CancellationToken ct);
        Task AddAsync(TEntity entity, CancellationToken ct);
        //void Remove(TEntity entity);
        //void Update(TEntity entity);
        Task SaveChangesAsync(CancellationToken ct);
        //Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct);
    }
}
