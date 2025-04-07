namespace MunicipalityTaxesAPI.Interfaces
{
    public interface IEntityFilter<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> ApplyRelations(IQueryable<TEntity> queryable);
        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> queryable, IBaseGetRequest request);
    }
}