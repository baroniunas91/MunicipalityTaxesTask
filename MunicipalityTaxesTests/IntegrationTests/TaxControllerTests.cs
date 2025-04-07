using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MunicipalityTaxesAPI.ContextConfiguration;
using MunicipalityTaxesAPI.Entities;
using MunicipalityTaxesAPI.Enums;
using MunicipalityTaxesAPI.Extensions;
using MunicipalityTaxesAPI.Models.Requests;
using MunicipalityTaxesAPI.Models.Responses;
using MunicipalityTaxesTests.IntegrationTests.Shared;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MunicipalityTaxesTests.IntegrationTests
{
    public class TaxControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public TaxControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _factory.ResetDatabase();
        }

        [Theory]
        [InlineData("2024-01-01", 0.1)]
        [InlineData("2024-03-16", 0.2)]
        [InlineData("2024-05-02", 0.4)]
        [InlineData("2024-07-10", 0.2)]
        public async Task GetSingleByQueryParams_ShoulReturnDailyTaxRate_WhenTaxGetRequestProvided(string date, double expectedTaxRate)
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();

            //Act
            var response = await client.GetAsync($"/taxes/single?municipality=Copenhagen&date={date}");
            var result = JsonConvert.DeserializeObject<TaxByMunicipalityandDateResponse>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Municipality.Should().Be("Copenhagen");
            result.TaxRate.Should().Be(expectedTaxRate);
        }

        [Fact]
        public async Task GetSingleByQueryParams_ShoulReturnNotFound_WhenTaxGetRequestProvided()
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();

            //Act
            var response = await client.GetAsync($"/taxes/single?municipality=Copenhagen&date=2023-02-01");
            var result = JsonConvert.DeserializeObject<TaxByMunicipalityandDateResponse>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("Kauno", TaxType.Daily, 0.25, "2025-04-10", "2025-04-11")]
        [InlineData("Vilniaus", TaxType.Weekly, 0.35, "2025-04-10", "2025-04-17")]
        [InlineData("Palangos", TaxType.Monthly, 0.35, "2025-07-01", "2025-08-01")]
        [InlineData("Kretingos", TaxType.Yearly, 0.15, "2025-01-01", "2026-01-01")]
        public async Task CreateTax_ShoulCreateTaxEntity_WhenCorrectTaxCreateRequestProvided(string municipality, TaxType type, double taxRate, string periodStart, string expectedPeriodEnd)
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();

            var taxCreateRequest = new TaxCreateRequest
            {
                Municipality = municipality,
                Type = type,
                TaxRate = taxRate,
                TaxSchedule = new TaxScheduleCreateRequest
                {
                    PeriodStart = DateTime.Parse(periodStart)
                }
            };

            var dataString = JsonConvert.SerializeObject(taxCreateRequest);
            var requestContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"/taxes", requestContent);
            var result = JsonConvert.DeserializeObject<TaxResponse>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            result.Should().NotBeNull();
            result.Municipality.Should().Be(municipality);
            result.Type.Should().Be(type.GetEnumMemberValue());
            result.TaxRate.Should().Be(taxRate);
            result.TaxSchedule.Should().NotBeNull();
            result.TaxSchedule.PeriodStart.Should().Be(DateTime.Parse(periodStart));
            result.TaxSchedule.PeriodEnd.Should().Be(DateTime.Parse(expectedPeriodEnd));
        }

        [Theory]
        [InlineData("", TaxType.Daily, 0.25, "2025-04-10")]
        [InlineData("Vilniaus", TaxType.Weekly, 0, "2025-04-10")]
        [InlineData("Palangos", TaxType.Monthly, 0.35, "1000-07-01")]
        [InlineData("Kretingos", TaxType.Yearly, 2, "2025-01-01")]
        public async Task CreateTax_ShoulReturnBadRequest_WhenInvalidTaxCreateRequestProvided(string municipality, TaxType type, double taxRate, string periodStart)
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();

            var taxCreateRequest = new TaxCreateRequest
            {
                Municipality = municipality,
                Type = type,
                TaxRate = taxRate,
                TaxSchedule = new TaxScheduleCreateRequest
                {
                    PeriodStart = DateTime.Parse(periodStart)
                }
            };

            var dataString = JsonConvert.SerializeObject(taxCreateRequest);
            var requestContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync($"/taxes", requestContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("Kauno", TaxType.Daily, 0.25, "2025-04-10", "2025-04-11")]
        [InlineData("Vilniaus", TaxType.Weekly, 0.35, "2025-04-10", "2025-04-17")]
        [InlineData("Palangos", TaxType.Monthly, 0.35, "2025-07-01", "2025-08-01")]
        [InlineData("Kretingos", TaxType.Yearly, 0.15, "2025-01-01", "2026-01-01")]
        public async Task UpdateTax_ShoulUpdateTaxEntity_WhenCorrectTaxUpdateRequestProvided(string municipality, TaxType type, double taxRate, string periodStart, string expectedPeriodEnd)
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var customTax = new TaxEntity()
            {
                Id = 123,
                Municipality = "London",
                Type = TaxType.Yearly,
                TaxRate = 0.2,
                TaxSchedule = new TaxScheduleEntity()
                {
                    PeriodStart = new DateTime(2025, 01, 01),
                    PeriodEnd = new DateTime(2026, 01, 01)
                }
            };

            context.Add(customTax);
            context.SaveChanges();

            var taxUpdateRequest = new TaxUpdateRequest
            {
                Municipality = municipality,
                Type = type,
                TaxRate = taxRate,
                TaxSchedule = new TaxScheduleUpdateRequest
                {
                    PeriodStart = DateTime.Parse(periodStart)
                }
            };

            var dataString = JsonConvert.SerializeObject(taxUpdateRequest);
            var requestContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"/taxes/{123}", requestContent);

            var updatedTax = await context.Taxes.Include(x => x.TaxSchedule).AsNoTracking().FirstOrDefaultAsync(x => x.Id == 123);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedTax.Should().NotBeNull();
            updatedTax.Municipality.Should().Be(municipality);
            updatedTax.Type.Should().Be(type);
            updatedTax.TaxRate.Should().Be(taxRate);
            updatedTax.TaxSchedule.Should().NotBeNull();
            updatedTax.TaxSchedule.PeriodStart.Should().Be(DateTime.Parse(periodStart));
            updatedTax.TaxSchedule.PeriodEnd.Should().Be(DateTime.Parse(expectedPeriodEnd));
        }

        [Theory]
        [InlineData("", TaxType.Daily, 0.25, "2025-04-10")]
        [InlineData("Vilniaus", TaxType.Weekly, 0, "2025-04-10")]
        [InlineData("Palangos", TaxType.Monthly, 2, "2025-07-01")]
        [InlineData("Kretingos", TaxType.Yearly, 0.15, "1000-01-01")]
        public async Task UpdateTax_ShoulReturnBadRequest_WhenInvalidTaxUpdateRequestProvided(string municipality, TaxType type, double taxRate, string periodStart)
        {
            // Arrange
            var client = _factory.CreateClient();
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            var customTax = new TaxEntity()
            {
                Id = 123,
                Municipality = "London",
                Type = TaxType.Yearly,
                TaxRate = 0.2,
                TaxSchedule = new TaxScheduleEntity()
                {
                    PeriodStart = new DateTime(2025, 01, 01),
                    PeriodEnd = new DateTime(2026, 01, 01)
                }
            };

            context.Add(customTax);
            context.SaveChanges();

            var taxUpdateRequest = new TaxUpdateRequest
            {
                Municipality = municipality,
                Type = type,
                TaxRate = taxRate,
                TaxSchedule = new TaxScheduleUpdateRequest
                {
                    PeriodStart = DateTime.Parse(periodStart)
                }
            };

            var dataString = JsonConvert.SerializeObject(taxUpdateRequest);
            var requestContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync($"/taxes/{123}", requestContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
