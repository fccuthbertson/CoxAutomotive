using System;
using System.Threading.Tasks;
using CoxAutomotive.Models.Domain;

namespace CoxAutomotive.Services
{
    public class GetDataSetId : IGetDataSet
    {
        private readonly IGetAutomotiveData _data;

        public GetDataSetId(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }

        public async Task<DataSetId> Get()
        {
            try
            
            {
                var id = await _data.GetDataSetId();
                if (id is null) throw new ArgumentNullException(nameof(DataSetId));
                return id;
            }
            catch (Exception)
            {
                // Log and or throw
                return null;
            }            
        }
    }
}
