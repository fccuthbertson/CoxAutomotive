using System.Collections.Generic;

namespace CoxAutomotive.Models.Http.Response
{
    public class InventoryResponse
    {
        public IEnumerable<DealerVehicles> DealerVehicles { get; set; }
    }

    public class DealerVehicles
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<VehicleResponse> Vehicles { get; set; }
    }


}
