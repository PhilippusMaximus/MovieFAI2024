using FAI.Application.Authentication;
using FAI.Application.Extensions;
using FAI.Common.Services;
using FAI.Persistence.Extensions;
using FAI.Persistence.Repositories.DBContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

                g.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authentictation header using basic scheme"
                });

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string []{}
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

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.RegisterServices();

            // Basic Authentication konfigurieren
            builder.Services.AddAuthentication(nameof(BasicAuthenticationHandler))
                            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(nameof(BasicAuthenticationHandler), null);


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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}


