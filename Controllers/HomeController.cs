using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoxAutomotive.Models;
using CoxAutomotive.Services;
using CoxAutomotive.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoxAutomotive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetDataSetId _getDataSet;
        private readonly IGetInventory _getInvenory;

        public HomeController(IGetDataSetId getDataSet,
                               IGetInventory getInvenory
                               )
        {
            _getDataSet = getDataSet;
            _getInvenory = getInvenory;
        }

        public async Task<IActionResult> Index()
        {
            var id = await _getDataSet.Get();
            var stopwatch = Stopwatch.StartNew();
            var inventory = await _getInvenory.Get(id);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
            var vm = new Home { Inventory = inventory, Elapsed = stopwatch.Elapsed };
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
