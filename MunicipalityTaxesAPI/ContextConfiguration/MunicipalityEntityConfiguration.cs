using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxesAPI.Entities;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class MunicipalityEntityConfiguration : IEntityTypeConfiguration<MunicipalityEntity>
    {
        public void Configure(EntityTypeBuilder<MunicipalityEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
