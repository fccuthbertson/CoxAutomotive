using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoxAutomotive.Models.Domain;
using CoxAutomotive.Models.Response;

namespace CoxAutomotive.Services
{
    public class GetDealer : IGetDealer
    {
        private readonly IGetAutomotiveData _data;

        public GetDealer(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }

        public async Task<Dealer> Get(DataSetId @in , DealerId dealerId)
        {
            try
            {
                var dealer = await _data.GetDealer(@in, dealerId);
                if (dealer == null) throw new ArgumentNullException(nameof(DataSetId) + nameof(DealerId));
                return dealer;
            }
            catch(Exception)
            {
                //Log and or throw exeption
                return null;
            }
        }
        
    }
}
