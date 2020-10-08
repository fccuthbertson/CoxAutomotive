using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;

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
            var model = new DataSetId
            {
                Value = @in.DataSetId
            };
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

    public interface IInventoryMapper : IMap<InventoryResponce, Inventory> { }
    public class InventoryMapper : IInventoryMapper
    {
        private readonly IVehicleMapper _vehicleMapper;

        public InventoryMapper(IVehicleMapper vehicleMapper)
        {
            _vehicleMapper = vehicleMapper;
        }
        public Inventory Map(InventoryResponce @in)
        {
            var model = new Inventory();
            if (@in is null || @in.DealerVehicles is null) return null;
            for (int i = 0; i < @in.DealerVehicles.Count; i++)
            {
                model.Dealers.Add(
                                     new Dealer
                                     {
                                         DealerId = @in.DealerVehicles[i].DealerId,
                                         Name = @in.DealerVehicles[i].Name,
                                         Vehicles = GetVehicles(@in.DealerVehicles[i].Vehicles)
                                     }
                                  );
            }

            return model;
        }
        public List<Vehicle> GetVehicles(List<VehicleResponse> vehicleResponses)
        {
            var vehicles = new List<Vehicle>();
                for (int j = 0; j < vehicleResponses.Count; j++)
                {
                    vehicles.Add(_vehicleMapper.Map(vehicleResponses[j]));
                }
            return vehicles;
        }

        
    }
}

   

