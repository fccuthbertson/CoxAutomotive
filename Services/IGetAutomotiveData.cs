using CoxAutomotive.Mappers;
using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Http.Response;
using CoxAutomotive.Models.Response;
using Newtonsoft.Json;
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
        Task<VehicleIds> GetVehicleIds(DataSetId dataSetId);
        Task<Inventory> GetInventory(DataSetId dataSetId);
        Task<Inventory> GetCheatInventory(DataSetIdCheat dataSetId);
    }

    public class GetAutomotiveData : IGetAutomotiveData
    {
        private readonly IDataSetIdMapper _dataSetMapper;
        private readonly IDealerMapper _dealerMapper;
        private readonly IDataSetVehiclesMapper _dataSetVehiclesMapper;
        private readonly IVehicleMapper _vehicleMapper;
        private readonly IInventoryMapper _inventoryMapper;
        private readonly IPathService _pathService;

        private readonly HttpClient _client;
        private const string Cox = "cox";

        public GetAutomotiveData(IHttpClientFactory httpClientFactory,
                                 IDataSetIdMapper dataSetMapper,
                                 IDataSetVehiclesMapper dataSetVehiclesMapper,
                                 IDealerMapper dealerMapper,
                                 IVehicleMapper vehicleMapper,
                                 IInventoryMapper inventoryMapper,
                                 IPathService pathService
                                 )
        {
            _client = httpClientFactory.CreateClient(Cox);
            _dataSetMapper = dataSetMapper;
            _dataSetVehiclesMapper = dataSetVehiclesMapper;
            _dealerMapper = dealerMapper;
            _vehicleMapper = vehicleMapper;
            _inventoryMapper = inventoryMapper;
            _pathService = pathService;
        }

        private async Task<TOut> Get<TOut, TResponse, TMapper, TPath>(TMapper mapper, params object[] args)
            where TMapper : IMap<TResponse, TOut>
        {
            var path = _pathService.GetPathFor<TPath>(args);
            var response = await _client.GetAsync(path);
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
            return await Get<DataSetId, DataSetResponse, IDataSetIdMapper, DataSetId>(_dataSetMapper);
        }

        public async Task<Dealer> GetDealer(DataSetId dataSetId, DealerId dealerId)
        {
            return await Get<Dealer, DealerResponse, IDealerMapper, DealerId>(_dealerMapper, dataSetId.Value, dealerId.Value);
        }

        public async Task<Vehicle> GetVehicle(DataSetId dataSetId, VehicleId vehicleId)
        {
            return await Get<Vehicle, VehicleResponse, IVehicleMapper, VehicleId>(_vehicleMapper, dataSetId.Value, vehicleId.Value);
        }


        public async Task<VehicleIds> GetVehicleIds(DataSetId dataSetId)
        {
            return await Get<VehicleIds, DataSetVehiclesResponse, IDataSetVehiclesMapper, Vehicle>(_dataSetVehiclesMapper, dataSetId.Value);
        }


        public async Task<Inventory> GetInventory(DataSetId dataSetId)
        {            
            var dataSetsVechicles = await GetVehicleIds(dataSetId);
            var carTasks = dataSetsVechicles.Value.Select(id => GetVehicle(dataSetId, new VehicleId(id)));
            var vehicles = await Task.WhenAll(carTasks);

            var dealerIds = vehicles.Select(v => v.DealerId).Distinct();
            var dealers =  await Task.WhenAll(dealerIds.Select(id => GetDealer(dataSetId, new DealerId(id))));

            var dealerList = dealers.Select(dealer => new Dealer
            {
                DealerId = dealer.DealerId,
                Name = dealer.Name,
                Vehicles = vehicles.Where(v => v.DealerId.Equals(dealer.DealerId))
            });

            return new Inventory { Dealers = dealerList };
        }


        public async Task<Inventory> GetCheatInventory(DataSetIdCheat dataSetId)
        {
            return await Get<Inventory, InventoryResponse, IInventoryMapper, DataSetIdCheat>(_inventoryMapper, dataSetId.Value);
        }
    }
}