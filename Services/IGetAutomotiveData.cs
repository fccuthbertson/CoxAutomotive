using CoxAutomotive.Mappers;
using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            var dataSetsVechicles = await GetDataSetVehicles(dataSetId);
            var carTasks = dataSetsVechicles.VehicleIds.Select(id => GetVehicle(dataSetId, new VehicleId(id)));
            var vehicles = await Task.WhenAll(carTasks);
            
            // get the Dealer Name and ID
            var dealerIds = vehicles.Select(v => v.DealerId).Distinct();
            var dealerTasks = dealerIds.Select(id => GetDealer(dataSetId, new DealerId(id)));
            return GetInventory(vehicles.ToList(), (await Task.WhenAll(dealerTasks)).ToList());
        }


        public async Task<Inventory> GetInventoryFromCheat(DataSetId dataSetId)
        {
            var url = ApiDataSetIDCheat(dataSetId);
            return await Get<Inventory, InventoryResponse, IInventoryMapper>(url, _inventoryMapper);
        }

        private Inventory GetInventory(List<Vehicle> vehicles, List<Dealer> dealers)
        {

            if (vehicles is null) throw new ArgumentNullException(nameof(List<Vehicle>));
            if (dealers is null) throw new ArgumentNullException(nameof(List<Dealer>));

            var dealerList = dealers.Select(dealer => new Dealer {
                DealerId = dealer.DealerId,
                Name = dealer.Name,
                Vehicles = vehicles.Where(v => v.DealerId.Equals(dealer.DealerId))
            });

            var inventory = new Inventory();
            inventory.Dealers = dealerList;
            return inventory;
        }
    }
}