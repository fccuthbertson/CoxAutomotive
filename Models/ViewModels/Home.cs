using CoxAutomotive.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoxAutomotive.Models.ViewModels
{
    public class Home
    {
        public Inventory Inventory { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
