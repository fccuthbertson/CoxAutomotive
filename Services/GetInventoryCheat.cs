using CoxAutomotive.Models.Domain;
using System;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public class GetInventoryCheat : IGetInventory
    {
        private readonly IGetAutomotiveData _data;

        public GetInventoryCheat(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }
        public async Task<Inventory> Get(DataSetId @in)
        {
            try 
            {
                var inventory = await _data.GetCheatInventory(@in as DataSetIdCheat);
                if (inventory is null) throw new ArgumentNullException(nameof(DataSetId));
                return inventory;
            }
            catch
            {
                // Log and or throw
                return null;
            }
        }
    }
}
