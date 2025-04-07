using AutoMapper;
using FluentAssertions;
using Moq;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;
using MunicipalityTaxesAPI.Services;

namespace MunicipalityTaxesTests.UnitTests
{
    public class TaxServiceTests
    {
        [Fact]
        public async Task GetByMunicipalityAndDateAsync_ShoulReturnCorrectTaxRate_WhenTaxGetRequestProvided()
        {
            // Arrange
            var taxGetRequest = new TaxGetRequest()
            {
                Municipality = "Copenhagen",
                Date = new DateTime(2014, 1, 1),
            };

            var dailyTax = new TaxEntity()
            {
                Municipality = "Copenhagen",
                Type = TaxType.Daily,
                TaxRate = 0.1,
                TaxSchedule = new TaxScheduleEntity()
                {
                    PeriodStart = new DateTime(2024, 01, 01),
                    PeriodEnd = new DateTime(2024, 01, 02)
                }
            };

            var montlyTax = new TaxEntity()
            {
                Municipality = "Copenhagen",
                Type = TaxType.Monthly,
                TaxRate = 0.4,
                TaxSchedule = new TaxScheduleEntity()
                {
                    PeriodStart = new DateTime(2024, 01, 01),
                    PeriodEnd = new DateTime(2024, 02, 01)
                }
            };


            var taxes = new List<TaxEntity>()
            {
                dailyTax,
                montlyTax
            };

            var taxResponse = new TaxByMunicipalityandDateResponse()
            {
                Municipality = "Copenhagen",
                TaxRate = 0.1
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<TaxByMunicipalityandDateResponse>(dailyTax)).Returns(taxResponse);

            var repo = new Mock<IRepository<TaxEntity>>();

            repo.Setup(x => x.GetAllAsync(taxGetRequest, It.IsAny<CancellationToken>())).ReturnsAsync(taxes);

            var taxService = new TaxService(mapperMock.Object, repo.Object);

            var result = await taxService.GetByMunicipalityAndDateAsync(taxGetRequest, CancellationToken.None);

            result.Should().NotBeNull();
            result.Municipality.Should().Be("Copenhagen");
            result.TaxRate.Should().Be(0.1);
        }

        [Theory]
        [InlineData(TaxType.Daily, "2024-01-01", "2024-01-02")]
        [InlineData(TaxType.Weekly, "2024-01-01", "2024-01-08")]
        [InlineData(TaxType.Weekly, "2024-01-10", "2024-01-17")]
        [InlineData(TaxType.Monthly, "2024-01-01", "2024-02-01")]
        [InlineData(TaxType.Yearly, "2024-01-01", "2025-01-01")]
        public void GetTaxSchedulePeriodEndDate_ShoulReturnCorrectPeriodEnd_WhenTaxTypeAndPeriodStartProvided(TaxType type, string periodStart, string expectedperiodEnd)
        {
            // Arrange
            var tax = new TaxEntity()
            {
                Municipality = "Copenhagen",
                Type = type,
                TaxRate = 0.1,
                TaxSchedule = new TaxScheduleEntity()
                {
                    PeriodStart = DateTime.Parse(periodStart)
                }
            };

            var mapperMock = new Mock<IMapper>();
            var repo = new Mock<IRepository<TaxEntity>>();

            var taxService = new TaxService(mapperMock.Object, repo.Object);

            var result = taxService.GetTaxSchedulePeriodEndDate(tax.Type, tax.TaxSchedule.PeriodStart);

            result.Should().Be(DateTime.Parse(expectedperiodEnd));
        }
    }
}
