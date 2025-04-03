using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxesAPI.Entities;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class TaxScheduleEntityConfiguration : IEntityTypeConfiguration<TaxScheduleEntity>
    {
        public void Configure(EntityTypeBuilder<TaxScheduleEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
