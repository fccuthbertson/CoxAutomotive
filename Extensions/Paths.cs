using CoxAutomotive.Models.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoxAutomotive.Extensions
{
    public static class Paths
    {
        public static void AddPaths(this IServiceCollection services)
        {
            services.AddSingleton<IPath, DataSetIdPath>();
            services.AddSingleton<IPath, DealerIdPath>();
            services.AddSingleton<IPath, VehicleIdPath>();
            services.AddSingleton<IPath, VehiclesPath>();
            services.AddSingleton<IPath, CheatPath>();
        }
    }
}
