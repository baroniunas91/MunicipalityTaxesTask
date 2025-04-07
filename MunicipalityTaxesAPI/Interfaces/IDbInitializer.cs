namespace MunicipalityTaxesAPI.Interfaces
{
    public interface IDbInitializer
    {
        void InitializeDatabase();
        void SeedDatabase();
    }
}
