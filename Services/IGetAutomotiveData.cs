using CoxAutomotive.Mappers;
using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public interface IGetAutomotiveData
    {
        Task<DataSetId> GetDataSetId();
        Task<Dealer> GetDealer(DataSetId dataSetId, DealerId dealerId);
        Task<Vehicle> GetVehicle(DataSetId dataSetId, VehicleId vehicleId);
    }

    public class GetAutomotiveData : IGetAutomotiveData
    {
        private readonly IDataSetMapper _dataSetMapper;
        private readonly IDealerMapper _dealerMapper;
        private readonly HttpClient _client;
        private const string Cox = "cox";
        private const string Api = "api";
        private string ApiDataSetId = $"{Api}/datasetId";
        private Func<DataSetId, DealerId, string> 
            ApiDataSetIdDealersDealerId = (DataSetId dataSetId, DealerId dealerId) => $"{Api}/{dataSetId.Value}/dealers/{dealerId.Value}";


        public GetAutomotiveData(IHttpClientFactory httpClientFactory, IDataSetMapper dataSetMapper)
        {
            _client = httpClientFactory.CreateClient(Cox);
            _dataSetMapper = dataSetMapper;


        }

        private async Task<TOut> Get<TOut, TResponse, TMapper>(string url, TMapper mapper)
            where TMapper :IMap<TResponse, TOut>
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
            return await Get<Dealer, DealerResponse, IDealerMapper>(ApiDataSetId, _dealerMapper);
        }

        public Task<Vehicle> GetVehicle(DataSetId dataSetId, VehicleId vehicleId)
        {
            throw new NotImplementedException();
        }
    }
}
