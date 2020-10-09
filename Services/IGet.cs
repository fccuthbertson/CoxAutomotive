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

    public interface IGetDataSetId : IGet<DataSetId>
    {

    }

    public interface IGetDealer : IGet<DataSetId, DealerId, Dealer>
    {

    }

    public interface IGetVehicle : IGet<DataSetId, VehicleId ,Vehicle>
    {

    }

    public interface IGetVehicleIds : IGet<DataSetId , VehicleIds>
    {
    
    }

    public interface IGetInventory : IGet<DataSetId, Inventory>
    { 
    
    }
}
