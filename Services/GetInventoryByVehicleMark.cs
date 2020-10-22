using CoxAutomotive.Models.Domain;
using System;
using System.Threading.Tasks;

namespace CoxAutomotive.Services
{
    public class GetInventoryByVehicleMark : IGetInventoryByMake
    {
        private readonly IGetInventoryData _data;

        public GetInventoryByVehicleMark(IGetInventoryData data)
        {
            _data = data;
        }

        public async Task<InventoryByVehicleMake> Get()
        {
            try
            {
                var result = await _data.GetInventoryByCarMake();
                if (result is null) return null;
                return result;
            }
            catch (Exception ex)
            {
                // log exeption
                return null;
            }
        }
    }
}
