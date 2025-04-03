using Microsoft.EntityFrameworkCore;
using MunicipalityTaxesAPI.Entities;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {  
        }

        public virtual DbSet<MunicipalityEntity> Municipalities { get; set; }
        public virtual DbSet<TaxEntity> Taxes { get; set; }
        public virtual DbSet<TaxScheduleEntity> TaxScheduleEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new MunicipalityEntityConfiguration());
            builder.ApplyConfiguration(new TaxEntityConfiguration());
            builder.ApplyConfiguration(new TaxScheduleEntityConfiguration());
        }
    }
}
