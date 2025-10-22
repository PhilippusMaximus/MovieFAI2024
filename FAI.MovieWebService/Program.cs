using FAI.Persistence.Repositories.DBContext;
using Microsoft.EntityFrameworkCore;
using FAI.Persistence.Extensions;
using FAI.Application.Extensions;

namespace FAI.MovieWebService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);       

            // Swagger OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();

            // Swagger Configuration
            builder.Services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "FAI Movie Web Service",
                    Version = "v1",
                    Description = "Web Service for managing movies, genres, and medium types.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "FAI Support",
                        Email = "philipp@rheinberger-it.at"
                    }
                });
            });

            // MovieDbContext konfigurieren
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContextProduction");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllers();

            // Registrieren der Repositories und Services via Reflection
            builder.Services.RegisterRepositories();
            builder.Services.RegisterServices();

            // Anwendung wird erstellt
            var app = builder.Build();

            // Nur wenn App in Development Modus gestartet wird
            if (app.Environment.IsDevelopment())
            {
                // Enable middleware to serve generated OpenAPI specification as a JSON endpoint.
                
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            // 
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}


