using CoxAutomotive.Models.Domain;
using System;

namespace CoxAutomotive.Models.ViewModel
{
    public class InventoryByVMake
    {
        public TimeSpan TimeSpan { get; set; }
        public InventoryByVehicleMake Makes { get; set; }
    }
}
