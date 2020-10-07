using System.Collections.Generic;

namespace CoxAutomotive.Models.Domain
{
    public class Dealer
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
