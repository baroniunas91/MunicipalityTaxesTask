namespace MunicipalityTaxesAPI.Interfaces
{
    // <summary>
    ///     Interface to control Entity filters and relations
    /// </summary>
    public interface IEntityFilter<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> ApplyRelations(IQueryable<TEntity> queryable);
        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> queryable, IBaseGetRequest request);
    }
}