using CoxAutomotive.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public class GetDataSetVehicles : IGetDataSetVehicles
    {
        private readonly IGetAutomotiveData _data;

        public GetDataSetVehicles(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }

        public async Task<DataSetVehicles> Get(DataSetId dataSetId)
        {
            try
            {
                var vehicles = await _data.GetDataSetVehicles(dataSetId);
                if (vehicles is null) throw new ArgumentNullException(nameof(DataSetId));
                return vehicles;
            }
            catch (Exception)
            {
                //Log and or throw 
                return null;
            }
        }
        // why I should have this here 
        public Task<DataSetId> Get()
        {
            throw new NotImplementedException();
        }
    }
}
