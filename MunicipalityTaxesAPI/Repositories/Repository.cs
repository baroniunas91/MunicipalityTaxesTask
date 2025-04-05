using Microsoft.EntityFrameworkCore;
using MunicipalityTaxesAPI.ContextConfiguration;
using MunicipalityTaxesAPI.Interfaces;
using System.Linq.Expressions;

namespace MunicipalityTaxesAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly MyDbContext _context;
        private readonly IEntityFilter<TEntity> _entityFilter;

        public Repository(MyDbContext context, IEntityFilter<TEntity> entityFilter)
        {
            _context = context;
            _entityFilter = entityFilter;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IBaseGetRequest baseGetRequest, CancellationToken ct)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = _entityFilter.ApplyRelations(query);
            query = _entityFilter.ApplyFilter(query, baseGetRequest);
            return await query.ToListAsync(ct);
        }

        public async Task AddAsync(TEntity entity, CancellationToken ct)
        {
            await _context.Set<TEntity>().AddAsync(entity, ct);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            query = _entityFilter.ApplyRelations(query);
            return await query.Where(expression).ToListAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
