using CoxAutomotive.Mappers;
using CoxAutomotive.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoxAutomotive.Extensions
{
    public static class Services
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IGetAutomotiveData, GetAutomotiveData>();
            services.AddSingleton<IGetDealer, GetDealer>();
            services.AddSingleton<IGetVehicle, GetVehicle>();
            services.AddSingleton<IGetDataSetId, GetDataSetId>();
            services.AddSingleton<IGetVehicleIds, GetDataSetVehicles>();
            services.AddSingleton<IDataSetIdMapper, DataSetMapper>();
            services.AddSingleton<IDealerMapper, DealerMapper>();
            services.AddSingleton<IVehicleMapper, Vehicleapper>();
            services.AddSingleton<IDataSetVehiclesMapper, DataSetVehiclesMapper>();
            services.AddSingleton<IInventoryMapper, InventoryMapper>();
            services.AddSingleton<IGetInventory, GetInventoryCheat>();
            // Register last so that the container returns this instace
            services.AddSingleton<IGetInventory, GetInventory>();

            services.AddSingleton<IPathService, PathService>();
            services.AddPaths();
        }
    }
}
