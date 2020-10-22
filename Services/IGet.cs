using CoxAutomotive.Models.Domain;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public interface IGet<T>
    {
        Task<T> Get();
    }
    public interface IGet<TIn, TOut>
    {
        
        Task<TOut> Get(TIn @in);
    }
    public interface IGet<TIn, VIn, TOut>
    {
        Task<TOut> Get(TIn @in, VIn vIn);
    }

    public interface IGetDataSet : IGet<DataSetId>
    {

    }

    public interface IGetDealer : IGet<DataSetId, DealerId, Dealer>
    {

    }

    public interface IGetVehicle : IGet<DataSetId, VehicleId ,Vehicle>
    {

    }

    public interface IGetDataSetVehicles : IGet<DataSetId , DataSetVehicles>
    {
    
    }

    public interface IGetInventoryCheat : IGet<DataSetId, Inventory>
    { 
    
    }

    public interface IGetInvenory : IGet<DataSetId, Inventory>
    { 
    
    }

    public interface IGetInventoryByMake : IGet<InventoryByVehicleMake>
    {
    }
}
