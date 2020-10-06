using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoxAutomotive.Models;
using CoxAutomotive.Services;

namespace CoxAutomotive.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetDataSet _getDataSet;

        public HomeController(IGetDataSet getDataSet)
        {
            _getDataSet = getDataSet;
        }

        public async Task<IActionResult> Index()
        {
            var id = await _getDataSet.Get();
            Console.WriteLine(id.Value);
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
