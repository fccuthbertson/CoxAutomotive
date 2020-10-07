using System.Collections.Generic;

namespace CoxAutomotive.Models.Response
{
    public class InventoryResponce
    {
        public List<DealerVehicles> DealerVehicles { get; set; }
    }

    public class DealerVehicles
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public List<VehicleResponse> Vehicles { get; set; }
    }

   
}
