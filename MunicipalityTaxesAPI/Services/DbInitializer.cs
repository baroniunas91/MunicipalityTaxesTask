using Microsoft.EntityFrameworkCore;
using MunicipalityTaxesAPI.ContextConfiguration;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly MyDbContext _dbContext;

        public DbInitializer(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.Migrate();
            
            if (_dbContext.Taxes.Any())
            {
                return;
            }

            var yearlyPeriodStart = new DateTime(2024, 1, 1);
            var yearlyPeriodEnd = yearlyPeriodStart.AddYears(1);

            var monthlyPeriodStart = new DateTime(2024, 5, 1);
            var monthlyPeriodEnd = monthlyPeriodStart.AddMonths(1);

            var dailyPeriodStart = new DateTime(2024, 1, 1);
            var dailyPeriodEnd = dailyPeriodStart.AddDays(1);

            var secondDailyPeriodStart = new DateTime(2024, 12, 25);
            var secondDailyPeriodEnd = secondDailyPeriodStart.AddDays(1);

            // Seed data
            var taxes = new List<TaxEntity>
            {
                new TaxEntity {
                    Municipality = "Copenhagen",
                    Type = Enums.TaxType.Yearly,
                    TaxRate = 0.2,
                    TaxSchedule = new TaxScheduleEntity(){
                        PeriodStart = yearlyPeriodStart,
                        PeriodEnd = yearlyPeriodEnd
                    }
                },
                new TaxEntity {
                    Municipality = "Copenhagen",
                    Type = Enums.TaxType.Monthly,
                    TaxRate = 0.4,
                    TaxSchedule = new TaxScheduleEntity(){
                        PeriodStart = monthlyPeriodStart,
                        PeriodEnd = monthlyPeriodEnd
                    }
                },
                new TaxEntity {
                    Municipality = "Copenhagen",
                    Type = Enums.TaxType.Daily,
                    TaxRate = 0.1,
                    TaxSchedule = new TaxScheduleEntity(){
                        PeriodStart = dailyPeriodStart,
                        PeriodEnd = dailyPeriodEnd
                    }
                },
                new TaxEntity {
                    Municipality = "Copenhagen",
                    Type = Enums.TaxType.Daily,
                    TaxRate = 0.1,
                    TaxSchedule = new TaxScheduleEntity(){
                        PeriodStart = secondDailyPeriodStart,
                        PeriodEnd = secondDailyPeriodEnd
                    }
                }
            };

            _dbContext.Taxes.AddRange(taxes);
            _dbContext.SaveChanges();
        }
    }
}
