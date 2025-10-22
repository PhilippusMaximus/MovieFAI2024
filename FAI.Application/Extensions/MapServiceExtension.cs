using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Application.Extensions
{
    public static class ServiceBuilderExtension
    {
        // 
        public static void RegisterServices(this IServiceCollection services)
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
