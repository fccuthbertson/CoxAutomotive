using CoxAutomotive.Mappers;
using CoxAutomotive.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoxAutomotive.Extentions
{
    public static class Services
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddSingleton<IGetAutomotiveData, GetAutomotiveData>();
            services.AddSingleton<IGetDealer, GetDealer>();
            services.AddSingleton<IGetVehicle, GetVehicle>();
            services.AddSingleton<IGetDataSet, GetDataSetId>();
            services.AddSingleton<IGetDataSetVehicles, GetDataSetVehicles>();
            services.AddSingleton<IDataSetMapper, DataSetMapper>();
            services.AddSingleton<IDealerMapper, DealerMapper>();
            services.AddSingleton<IVehicleMapper, Vehicleapper>();
            services.AddSingleton<IDataSetVehiclesMapper, DataSetVehiclesMapper>();
            services.AddSingleton<IInventoryMapper, InventoryMapper>();
            services.AddSingleton<IGetInventoryCheat, GetInventoryCheat>();
            services.AddSingleton<IGetInvenory, GetInventory>();
            services.AddSingleton<IGetInventoryByMake, GetInventoryByVehicleMark>();
            services.AddSingleton<IGetInventoryData, GetInventoryData>();

        }
    }
}
