using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MunicipalityTaxesAPI.Entities;

namespace MunicipalityTaxesAPI.ContextConfiguration
{
    public class TaxScheduleEntityConfiguration : IEntityTypeConfiguration<TaxScheduleEntity>
    {
        public void Configure(EntityTypeBuilder<TaxScheduleEntity> builder)
        {
            builder.ToTable("tax_schedules");

            builder.Property(e => e.Id).HasColumnName("id");
            builder.Property(e => e.PeriodStart).HasColumnName("period_start").HasColumnType("datetime");
            builder.Property(e => e.PeriodEnd).HasColumnName("period_end").HasColumnType("datetime");
            builder.Property(e => e.TaxId).HasColumnName("tax_id");
            builder.HasOne(x => x.Tax)
                .WithOne(x => x.TaxSchedule)
                .HasForeignKey<TaxScheduleEntity>(x => x.TaxId)
                .IsRequired();
        }
    }
}
