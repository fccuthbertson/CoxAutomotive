using CoxAutomotive.Models.ViewModel;
using CoxAutomotive.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CoxAutomotive.Controllers
{
    public class InventoryByMakeController : Controller
    {
        private readonly IGetInventoryByMake _getInventoryByMake;
        public InventoryByMakeController(IGetInventoryByMake getInventoryByMake)
        {
            _getInventoryByMake = getInventoryByMake;
        }

    
        public async Task<IActionResult> Inventory()
        {
            var stopwatch = Stopwatch.StartNew();
            var carsByMake = await _getInventoryByMake.Get();
            stopwatch.Stop();
            var vm = new InventoryByVMake { TimeSpan = stopwatch.Elapsed, Makes = carsByMake };
            return View(vm);
        }
    }
}
