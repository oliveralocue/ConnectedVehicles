using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ConnectedVehicles.Models;
using ConnectedVehicles.Repository;
using ConnectedVehicles.DTOs;

namespace ConnectedVehicles.Controllers
{
    public class HomeController : Controller
    {
        
        VehiclesRepository repo;
        public HomeController(VehiclesRepository repo)
        {
            
            this.repo = repo;

        }

        public async Task<IActionResult> Index()
        {

            var vm = await repo.GetAllItems();
            return View(vm);
        }

        public async Task<IActionResult> Client(string id)
        {
            try
            {
                var vm = await
                   repo.GetById<DetailVehicleDTO, string>(id);
                return View(vm);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AllStatus()
        {
            var vm = await repo.GetAllItems();
            return View(vm);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
