using Microsoft.EntityFrameworkCore;
using MunicipalityTaxesAPI.ContextConfiguration;
using MunicipalityTaxesAPI.Interfaces;
using MunicipalityTaxesAPI.Services;


namespace MunicipalityTaxesAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<MyDbContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IDbInitializer, DbInitializer>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<ITaxService, TaxService>();
            services.AddTransient<IDbInitializer, DbInitializer>();
        }

        public static void AddServicesFromAssemblies(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IRepository<>))
                .AddClasses(c => c.AssignableTo(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IEntityFilter<>))
                .AddClasses(c => c.AssignableTo(typeof(IEntityFilter<>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IFluentValidator))
                .AddClasses(classes => classes.AssignableTo<IFluentValidator>())
                .AsSelf()
                .WithTransientLifetime());
        }

        public static void ConfigureNewtonsoftJson(this IMvcBuilder builder)
        {
            builder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
            });
        }
    }
}