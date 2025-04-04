
using MunicipalityTaxesAPI.Extensions;
using MunicipalityTaxesAPI.Middlewares;
using System.Reflection;

namespace MunicipalityTaxesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddMvc().ConfigureNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddServices();
            builder.Services.AddServicesFromAssemblies();
            builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
            builder.Services.AddValidators();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.SeedDatabase();

            app.UseAuthorization();

            app.UseMiddleware<HttpStatusExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
