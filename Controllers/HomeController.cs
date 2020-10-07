using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoxAutomotive.Models;
using CoxAutomotive.Services;

namespace CoxAutomotive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetDataSet _getDataSet;
        private readonly IGetDealer _getDealer;
        private readonly IGetDataSetVehicles _getDataSetVehicles;
        private readonly IGetVehicle _getVehicle;
        private readonly IGetInventoryCheat _getInventoryCheat;

        public HomeController(IGetDataSet getDataSet,
                               IGetDealer getDealer,
                               IGetDataSetVehicles getDataSetVehicles,
                               IGetVehicle getVehicle,
                               IGetInventoryCheat getInventoryCheat
                               )
        {
            _getDataSet = getDataSet;
            _getDealer = getDealer;
            _getDataSetVehicles = getDataSetVehicles;
            _getVehicle = getVehicle;
            _getInventoryCheat = getInventoryCheat;
        }

        public async Task<IActionResult> Index()
        {
            var id = await _getDataSet.Get();
            Console.WriteLine(id.Value);
            // get DataSet's vehicles
          //  var dataSetVehicles = await _getDataSetVehicles.Get(id);

            // Delear info why I get error here it cannot notice which intefrace to chose
           // var delear = await _getDealer.Get(new DataSetId {Value = "Gle6pzVq2Ag" }, new DealerId { Value = 398581158 } );

            // get Vehicle data
          //  var vehicleDetails = await _getVehicle.Get(new DataSetId { Value = "V4UeI0Jp2Ag" }, new VehicleId { Value = 106850931 });

            //get inventory 
            var inventory = await _getInventoryCheat.Get(id);
            return View();
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
