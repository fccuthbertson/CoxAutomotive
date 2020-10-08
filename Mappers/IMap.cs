using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;
using System.Linq;

namespace CoxAutomotive.Mappers
{
    public interface IMap<TIn, TOut>
    {
        TOut Map(TIn @in);
    }

    public interface IDataSetMapper : IMap<DataSetResponse, DataSetId> { }
    public class DataSetMapper : IDataSetMapper
    {
        public DataSetId Map(DataSetResponse @in)
        {
            var model = new DataSetId(@in.DataSetId);
            return model;
        }
    }

    public interface IDealerMapper : IMap<DealerResponse, Dealer> { }
    public class DealerMapper : IDealerMapper
    {
        public Dealer Map(DealerResponse @in)
        {
            var model = new Dealer
            {
                DealerId = @in.DealerId,
                Name = @in.Name,


            };
            return model;
        }
    }

    public interface IVehicleMapper : IMap<VehicleResponse, Vehicle> { }
    public class Vehicleapper : IVehicleMapper
    {
        public Vehicle Map(VehicleResponse @in)
        {
            var model = new Vehicle
            {
                Model = @in.Model,
                Make = @in.Make,
                VehicleId = @in.VehicleId,
                Year = @in.Year,
                DealerId = @in.DealerId
            };
            return model;
        }
    }

    public interface IDataSetVehiclesMapper : IMap<DataSetVehiclesResponse, DataSetVehicles> { }
    public class DataSetVehiclesMapper : IDataSetVehiclesMapper
    {
        public DataSetVehicles Map(DataSetVehiclesResponse @in)
        {
            var model = new DataSetVehicles
            {
                VehicleIds = @in.VehicleIds
            };
            return model;
        }
    }

    public interface IInventoryMapper : IMap<InventoryResponse, Inventory> { }
    public class InventoryMapper : IInventoryMapper
    {
        private readonly IVehicleMapper _vehicleMapper;

        public InventoryMapper(IVehicleMapper vehicleMapper)
        {
            _vehicleMapper = vehicleMapper;
        }
        public Inventory Map(InventoryResponse @in)
        {
            var model = new Inventory();
            if (@in is null || @in.DealerVehicles is null) return null;
            var dealers = @in.DealerVehicles.Select(dv => new Dealer {
                DealerId = dv.DealerId,
                Name = dv.Name,
                Vehicles = dv.Vehicles.Select(_vehicleMapper.Map)
            });

            return model;
        }
    }
}

   

