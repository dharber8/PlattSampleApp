using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlattSampleApp.Models;
using PlattSampleApp;
using System.Threading.Tasks;

namespace PlattSampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async  Task<ActionResult> GetAllPlanets()
        {   
            var model = new AllPlanetsViewModel();
            model = await StarWarsProcessor.LoadAllPlanets();
            

            return View(model);
        }

        public async Task<ActionResult> GetPlanetTwentyTwo(int planetid)
        {

            var model = new SinglePlanetViewModel();
            model = await StarWarsProcessor.LoadSinglePlanet(planetid);


            return View(model);
        }

        public async Task<ActionResult> GetResidentsOfPlanetNaboo(string planetname)
        {
            var model = new PlanetResidentsViewModel();

            model = await StarWarsProcessor.LoadPlanetResidents(planetname);


            // TODO: Implement this controller action

            return View(model);
        }

        public async Task<ActionResult> VehicleSummary()
        {
            var model = new VehicleSummaryViewModel();

            // TODO: Implement this controller action
            model = await StarWarsProcessor.LoadAllVehicles();

            return View(model);
        }

      
    }
}