using CoxAutomotive.Mappers;
using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public interface IGetAutomotiveData
    {
        Task<DataSetId> GetDataSetId();
        Task<Dealer> GetDealer(DataSetId dataSetId, DealerId dealerId);
        Task<Vehicle> GetVehicle(DataSetId dataSetId, VehicleId vehicleId);
        Task<DataSetVehicles> GetDataSetVehicles(DataSetId dataSetId);
        Task<Inventory> GetDataSetDealersVehicle(DataSetId dataSetId);
        Task<Inventory> GetInventoryFromCheat(DataSetId dataSetId);
    }

    public class GetAutomotiveData : IGetAutomotiveData
    {
        private readonly IDataSetMapper _dataSetMapper;
        private readonly IDealerMapper _dealerMapper;
        private readonly IDataSetVehiclesMapper _dataSetVehiclesMapper;
        private readonly IVehicleMapper _vehicleMapper;
        private readonly IInventoryMapper _inventoryMapper;

        private readonly HttpClient _client;
        private const string Cox = "cox";
        private const string Api = "api";
        private string ApiDataSetId = $"{Api}/datasetId";
        private Func<DataSetId, DealerId, string>
            ApiDataSetIdDealersDealerId = (DataSetId dataSetId, DealerId dealerId) => $"{Api}/{dataSetId.Value}/dealers/{dealerId.Value}";

        private Func<DataSetId, string>
            ApiDataSetIDVehicles = (DataSetId dataSetId) => $"{Api}/{dataSetId.Value}/vehicles";

        private Func<DataSetId, VehicleId, string>
             ApiDataSetIdVehicleID = (DataSetId dataSetId, VehicleId vehicleId) => $"{Api}/{dataSetId.Value}/vehicles/{vehicleId.Value}";

        private Func<DataSetId, string>
            ApiDataSetIDCheat = (DataSetId dataSetId) => $"{Api}/{dataSetId.Value}/cheat";

        public GetAutomotiveData(IHttpClientFactory httpClientFactory,
                                 IDataSetMapper dataSetMapper,
                                 IDataSetVehiclesMapper dataSetVehiclesMapper,
                                 IDealerMapper dealerMapper,
                                 IVehicleMapper vehicleMapper,
                                 IInventoryMapper inventoryMapper)
        {
            _client = httpClientFactory.CreateClient(Cox);
            _dataSetMapper = dataSetMapper;
            _dataSetVehiclesMapper = dataSetVehiclesMapper;
            _dealerMapper = dealerMapper;
            _vehicleMapper = vehicleMapper;
            _inventoryMapper = inventoryMapper;
        }

        private async Task<TOut> Get<TOut, TResponse, TMapper>(string url, TMapper mapper)
            where TMapper : IMap<TResponse, TOut>
        {
            var response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<TResponse>(result);
                return mapper.Map(responseModel);
            }
            return default;
        }

        public async Task<DataSetId> GetDataSetId()
        {
            return await Get<DataSetId, DataSetResponse, IDataSetMapper>(ApiDataSetId, _dataSetMapper);
        }

        public async Task<Dealer> GetDealer(DataSetId dataSetId, DealerId dealerId)
        {
            var url = ApiDataSetIdDealersDealerId(dataSetId, dealerId);
            return await Get<Dealer, DealerResponse, IDealerMapper>(url, _dealerMapper);
        }

        public async Task<Vehicle> GetVehicle(DataSetId dataSetId, VehicleId vehicleId)
        {
            var url = ApiDataSetIdVehicleID(dataSetId, vehicleId);
            return await Get<Vehicle, VehicleResponse, IVehicleMapper>(url, _vehicleMapper);
        }


        public async Task<DataSetVehicles> GetDataSetVehicles(DataSetId dataSetId)
        {
            var url = ApiDataSetIDVehicles(dataSetId);
            return await Get<DataSetVehicles, DataSetVehiclesResponse, IDataSetVehiclesMapper>(url, _dataSetVehiclesMapper);
        }


        public async Task<Inventory> GetDataSetDealersVehicle(DataSetId dataSetId)
        {
            var cars = new ConcurrentBag<Vehicle>();
            var dataSetsVechicles = await GetDataSetVehicles(dataSetId);
           
              Parallel.For(0, dataSetsVechicles.VehicleIds.Length, async i =>
                {
                    cars.Add(await GetVehicle(dataSetId, new VehicleId { Value = dataSetsVechicles.VehicleIds[i] }));
                });

            // get the Dealer Name and ID
      
            var distinticDealrs = GetDealerListOfDataSetID(cars.ToList());
            var dealers = new ConcurrentBag<Dealer>();
            Parallel.For(0, distinticDealrs.Count - 1, async index =>
            {
                dealers.Add(await GetDealer(dataSetId, distinticDealrs[index]));
            }
            );

            return GetInventory(cars.ToList(), dealers.ToList());
          
        }


        public async Task<Inventory> GetInventoryFromCheat(DataSetId dataSetId)
        {
            var url = ApiDataSetIDCheat(dataSetId);
            return await Get<Inventory, InventoryResponce, IInventoryMapper>(url, _inventoryMapper);
        }

        private List<DealerId> GetDealerListOfDataSetID(List<Vehicle> vehicles)
        {
            if (vehicles is null) return null;

            var dealerIdArr = vehicles.Select(v => v.DealerId).ToArray().Distinct();
            var dealerIds = dealerIdArr.Select(v => new DealerId { Value = v }).ToList();
            return dealerIds;
        }

        private Inventory GetInventory(List<Vehicle> vehicles, List<Dealer> dearls)
        {

            if (vehicles is null) throw new ArgumentNullException(nameof(List<Vehicle>));
            if (dearls is null) throw new ArgumentNullException(nameof(List<Dealer>));

            var inventory = new Inventory();
            var dealerList = new List<Dealer>();
            for (int i = 0; i < dearls.Count; i++)
            {
               dealerList.Add(new Dealer
                {
                    DealerId = dearls[i].DealerId,
                    Name = dearls[i].Name,
                    Vehicles = vehicles.Where(v => v.DealerId == dearls[i].DealerId).ToList()
                });
            }
            inventory.Dealers = dealerList;
            return inventory;
        }
    }
}