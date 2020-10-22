using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoxAutomotive.Models;
using CoxAutomotive.Services;
using CoxAutomotive.Models.ViewModel;

namespace CoxAutomotive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetDataSet _getDataSet;
        private readonly IGetInvenory _getInvenory;
        private readonly IGetInventoryByMake _getInventoryByMake;

        public HomeController(IGetDataSet getDataSet,                          
                               IGetInvenory getInvenory,
                               IGetInventoryByMake getInventoryByMake
                               )
        {
            _getDataSet = getDataSet;
            _getInvenory = getInvenory;
            _getInventoryByMake = getInventoryByMake;
        }

        public async Task<IActionResult> Index()
        {
            var id = await _getDataSet.Get();
            Console.WriteLine(id.Value);
          
            var stopwatch = Stopwatch.StartNew();
            var inventory = await _getInvenory.Get(id);
            stopwatch.Stop();

            var vm = new Home { Inventory = inventory, TimeSpanToGetRecords = stopwatch.Elapsed };
            Console.WriteLine(stopwatch.Elapsed);
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
