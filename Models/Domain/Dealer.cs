using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Models.Domain
{
    public class Dealer
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
