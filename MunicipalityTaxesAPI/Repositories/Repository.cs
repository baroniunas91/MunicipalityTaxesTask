using MunicipalityTaxesAPI.ContextConfiguration;
using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly MyDbContext _context;

        public Repository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
