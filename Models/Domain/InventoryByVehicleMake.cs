using System.Collections.Generic;

namespace CoxAutomotive.Models.Domain
{
    public class InventoryByVehicleMake
    {
        ///
        //Toyota
        //    Camery 2 Dealer1
        //    Camery 3 dealer34
        //    corolla 2
        //BMW 
        //     Model1 32 Dealer10 
        ///

        public List<InventoryByMake> Makes { get; set; }
    }

    public class InventoryByMake
    { 
        public string Make { get; set; }
        public List<MakeDetails> MakeDetails { get; set; }
    }

    public class MakeDetails
    {
        public string Model { get; set; }
        public int Count { get; set; }
        public string DealerName { get; set; }
    }


    public class FlatVehicleDetails
    {
        public int VehicleId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int DealerId { get; set; }
        public string Name { get; set; }
        public int CountInDealer { get; set; }
    }

}
