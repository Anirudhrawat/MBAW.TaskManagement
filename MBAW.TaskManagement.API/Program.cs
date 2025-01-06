using MBAW.TaskManagement.API.Extentions;
using MBAW.TaskManagement.Application;
using MBAW.TaskManagement.Domain;
using MBAW.TaskManagement.Infrastructure;
using MBAW.TaskManagement.Mapping;

namespace MBAW.TaskManagement.API
{
    public class Program
    {
        private static void ConfigureServices(IServiceCollection serviceCollection, ConfigurationManager configurationManager)
        {
            serviceCollection.AddInfrastructureLayer(configurationManager);
            serviceCollection.AddStoredProcedureRegistry<Registry>();
            serviceCollection.AddApplicationLayer(configurationManager);
            serviceCollection.AddMappingLayer();
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Services.StartSeeding(typeof(Registry));
            app.UseExceptionMiddleware();

            app.Run();
        }
    }
}
