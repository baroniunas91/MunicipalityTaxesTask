using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxesAPI.Entities;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class TaxEntityConfiguration : IEntityTypeConfiguration<TaxEntity>
    {
        public void Configure(EntityTypeBuilder<TaxEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
