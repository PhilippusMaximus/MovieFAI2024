using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Persistence.Extensions
{
    public static class ServiceBuilderExtension
    {
        // Registers all repositories with the MapServiceDependencyAttribute
        public static void RegisterRepositories(this IServiceCollection services)
        {
            // Scrutor extension .Scan  
            services.Scan(s =>
            {
                s.FromAssemblies(Assembly.GetExecutingAssembly())
                .AddClasses(c => c.WithAttribute<Core.Attributes.MapServiceDependencyAttribute>())
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            });
        }
    }
}
