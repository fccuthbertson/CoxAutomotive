using CoxAutomotive.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public interface IGetInventoryData
    {       
        Task<InventoryByVehicleMake> GetInventoryByCarMake();
    }
    public class GetInventoryData : IGetInventoryData
    {
        private readonly IGetAutomotiveData _getAutomotiveData;
        public GetInventoryData(IGetAutomotiveData getAutomotiveData)
        {
            _getAutomotiveData = getAutomotiveData;
        }
       
        public async Task<InventoryByVehicleMake> GetInventoryByCarMake()
        { ///
            //Toyota
            //    Camery 2 Dealer1
            //    Camery 3 dealer34
            //    corolla 2
            //BMW 
            //     Model1 32 Dealer10 

            var dataSetID = await _getAutomotiveData.GetDataSetId();

            var dataSetVehicleIds = await _getAutomotiveData.GetDataSetVehicles(dataSetID);
            var vehiclesTasks = dataSetVehicleIds.VehicleIds.Select(id => _getAutomotiveData.GetVehicle(dataSetID, new VehicleId(id)));
            var vehicles = await Task.WhenAll(vehiclesTasks);
            var dealerIds = vehicles.Select(v => v.DealerId).Distinct();
            var dealerTasks = dealerIds.Select(id => _getAutomotiveData.GetDealer(dataSetID, new DealerId(id)));
            var dealers = await Task.WhenAll(dealerTasks);
            //group by the dat based on the Make
            return GetVehicleByMake(vehicles.ToList(), dealers.ToList());

        }

        private InventoryByVehicleMake GetVehicleByMake(List<Vehicle> vehicles, List<Dealer> dealers)
        {
            if (vehicles is null) throw new ArgumentNullException(nameof(List<Vehicle>));
            if (dealers is null) throw new ArgumentNullException(nameof(List<Dealer>));

            var flatVehicleDetails = vehicles.Select(v => new FlatVehicleDetails
            {
                DealerId = v.DealerId,
                Make = v.Make,
                Model = v.Model,
                Name = dealers.FirstOrDefault(d => d.DealerId == v.DealerId).Name,
                Year = v.Year,
                VehicleId = v.VehicleId,
                CountInDealer = vehicles.Count(vd => vd.Make == v.Make &&
                                                     vd.Model == v.Model &&
                                                     vd.DealerId == v.DealerId
                                                )
            });

            InventoryByVehicleMake inventoryByVehicleMark = new InventoryByVehicleMake();
            var marks = vehicles.Select(v => v.Make).Distinct();
            var result = marks.Select(m => new InventoryByMake
            {
                Make = m,
                MakeDetails = flatVehicleDetails.Where(v => v.Make.Equals(m)).Select(vv =>
                new MakeDetails
                {
                    Count = flatVehicleDetails.Count(vvv => vvv.Model.Equals(vv.Model) && vvv.DealerId.Equals(vv.DealerId) && vvv.Make.Equals(vv.Make)),
                    DealerName = vv.Name,
                    Model = vv.Model
                }
                ).ToList()
            }).ToList();

            inventoryByVehicleMark.Makes = result;

            return inventoryByVehicleMark;
        }
    }
}
