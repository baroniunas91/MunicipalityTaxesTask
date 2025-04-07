using MunicipalityTaxesAPI.Interfaces;

namespace MunicipalityTaxesAPI.Extensions
{
    public static class AppExtensions
    {
        public static void SeedDatabase(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbInitializer = services.GetRequiredService<IDbInitializer>();
            dbInitializer.InitializeDatabase();
        }
    }
}
