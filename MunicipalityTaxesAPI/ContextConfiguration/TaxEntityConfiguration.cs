using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Extensions;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class TaxEntityConfiguration : IEntityTypeConfiguration<TaxEntity>
    {
        public void Configure(EntityTypeBuilder<TaxEntity> builder)
        {
            builder.ToTable("taxes");
            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.Municipality).HasColumnName("municipality").HasColumnType("varchar(255)").IsRequired();
            builder.Property(e => e.Type).HasColumnName("type")
            .HasConversion(
                v => v.GetEnumMemberValue(),
                v => EnumExtensions.ParseByAttribute<TaxType>(v))
            .IsRequired();
            builder.Property(e => e.TaxRate).HasColumnName("rate");
            builder.HasOne(x => x.TaxSchedule)
                .WithOne(x => x.Tax)
                .HasForeignKey<TaxScheduleEntity>(x => x.TaxId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
