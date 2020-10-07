using System;
using System.Threading.Tasks;
using CoxAutomotive.Models.Domain;

namespace CoxAutomotive.Services
{
    public class GetVehicle : IGetVehicle
    {
        private readonly IGetAutomotiveData _data;

        public GetVehicle(IGetAutomotiveData getAutomotiveData)
        {
            _data = getAutomotiveData;
        }

        public async Task<Vehicle> Get(DataSetId @in, VehicleId vIn)
        {
            try
            {
                var vehicle = await _data.GetVehicle(@in, vIn);
                if (vehicle is null) throw new ArgumentNullException(nameof(DataSetId), nameof(VehicleId));
                return vehicle;
            }
            catch
            {
                // Log and throw
                return null;
            }
        }
    }
}
