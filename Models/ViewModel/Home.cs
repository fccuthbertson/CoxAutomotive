using CoxAutomotive.Models.Domain;
using System;

namespace CoxAutomotive.Models.ViewModel
{
    public class Home
    {
        public TimeSpan TimeSpanToGetRecords { get; set; }
        public Inventory Inventory { get; set; }
    }
}
