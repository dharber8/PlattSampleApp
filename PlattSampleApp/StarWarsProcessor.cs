using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using PlattSampleApp.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattSampleApp
{
    public class StarWarsProcessor
    {

        public static async Task<AllPlanetsViewModel> LoadAllPlanets()
        {
            //get all planets
            List<PlanetModel> planetList = new List<PlanetModel>();
            AllPlanetsViewModel endResult = new AllPlanetsViewModel();

            string url = $"{ ApiHelper.ApiClient.BaseAddress}planets/";
            double averageDiameter = 0.0;
            double totalDiameter = 0.0;
            int unknowns = 0;

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    AllPlanetsModel planets = await response.Content.ReadAsAsync<AllPlanetsModel>();

                    planetList.AddRange(planets.PlanetList);

                    while (planets.Next != null)
                    {

                        using (HttpResponseMessage nextPlanetResponse = await ApiHelper.ApiClient.GetAsync(planets.Next))
                        {
                            if (nextPlanetResponse.IsSuccessStatusCode)
                            {
                                planets = await nextPlanetResponse.Content.ReadAsAsync<AllPlanetsModel>();

                                planetList.AddRange(planets.PlanetList);
                            }

                        }
                    }

                    List<PlanetDetailsViewModel> unknownList = new List<PlanetDetailsViewModel>();

                    foreach (PlanetModel planet in planetList)
                    {
                        if (planet.Diameter != "unknown")
                        {
                            totalDiameter += Double.Parse(planet.Diameter);
                            endResult.Planets.Add(new PlanetDetailsViewModel()
                            {
                                Name = planet.Name,
                                LengthOfYear = planet.OrbitalPeriod,
                                Diameter = planet.Diameter,
                                Terrain = planet.Terrain,
                                Population = planet.Population,
                            });
                        }
                        else
                        {
                            unknownList.Add(new PlanetDetailsViewModel()
                            {
                                Name = planet.Name,
                                LengthOfYear = planet.OrbitalPeriod,
                                Diameter = planet.Diameter,
                                Terrain = planet.Terrain,
                                Population = planet.Population,
                            });
                            unknowns++; //add the unkown planets to subtract from count later
                        }
                            
                        

                    }
                    
                    averageDiameter = (totalDiameter / Double.Parse(planets.Count) - unknowns); //total diameter/total planets - unknowns
                    endResult.AverageDiameter = averageDiameter.ToString("N2");
                    endResult.Planets = endResult.Planets.OrderByDescending(o => o.Diameter).ToList(); // sort by diameter descending
                    endResult.Planets.AddRange(unknownList); //add unknowns to end of list
                    return endResult;
                } 
                else
                {

                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<SinglePlanetViewModel> LoadSinglePlanet(int planetid)
        {

            string url = $"{ ApiHelper.ApiClient.BaseAddress}planets/{ planetid }/"; //get all planets

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    PlanetModel planet = await response.Content.ReadAsAsync<PlanetModel>();

                    SinglePlanetViewModel endResult = new SinglePlanetViewModel()
                    {
                        Name = planet.Name,
                        LengthOfDay = planet.RotationPeriod,
                        LengthOfYear = planet.OrbitalPeriod,
                        Diameter = planet.Diameter,
                        Climate = planet.Climate,
                        Gravity = planet.Gravity,
                        Population = planet.Population,
                        SurfaceWaterPercentage = planet.SurfaceWater,

                    };

                    return endResult;
                } else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }

        public static async Task<PlanetModel> LoadSinglePlanet(string planetName) //overload for planetName
        {
            string url = $"{ ApiHelper.ApiClient.BaseAddress}planets/?search={ planetName }/"; //get all planets whose name is planetName

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    PlanetModel planets = await response.Content.ReadAsAsync<PlanetModel>();

                    return planets;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }

        public static async Task<PlanetResidentsViewModel> LoadPlanetResidents(string planetName)
        {
            List<ResidentSummary> residents = new List<ResidentSummary>();
            string url = $"{ ApiHelper.ApiClient.BaseAddress}planets/";//?search={ planetName }"; //get all planets

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    AllPlanetsModel planets = await response.Content.ReadAsAsync<AllPlanetsModel>();
                    
                    PlanetModel planet = planets.PlanetList[0]; // get the planet that is returned by name
                    
                    foreach (string resident in planet.Residents)
                    {
                        using (HttpResponseMessage peopleResponse = await ApiHelper.ApiClient.GetAsync(resident))
                        {
                            PeopleModel person = await peopleResponse.Content.ReadAsAsync<PeopleModel>(); //call API URI provided by the JSON result of the PlanetModel

                            if (peopleResponse.IsSuccessStatusCode)
                            {
                                residents.Add(new ResidentSummary()
                                {
                                    Name = person.Name,
                                    EyeColor = person.EyeColor,
                                    Height = person.Height,
                                    Weight = person.Mass,
                                    Gender = person.Gender,
                                    HairColor = person.HairColor,
                                    SkinColor = person.SkinColor,

                                });
                            } 
                            else
                            {
                                throw new Exception(response.ReasonPhrase);
                            }

                        }
                    }

                    PlanetResidentsViewModel endResult = new PlanetResidentsViewModel();

                    endResult.Residents = residents;

                    return endResult;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }

        public static async Task<VehicleSummaryViewModel> LoadAllVehicles() //Return List of Vehicles and count
        {
            VehicleSummaryViewModel endResult = new VehicleSummaryViewModel();
            List<VehicleModel> vehicles = new List<VehicleModel>();
            string url = $"{ ApiHelper.ApiClient.BaseAddress}vehicles/";
            string count = "";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {

                    AllVehiclesModel allVehicleResult = await response.Content.ReadAsAsync<AllVehiclesModel>();
                    

                    vehicles.AddRange(allVehicleResult.VehicleList.Where<VehicleModel>(v => v.Cost != "unknown"));

                    while (allVehicleResult.Next != null)
                    {
                        using (HttpResponseMessage nextVehicleResponse = await ApiHelper.ApiClient.GetAsync(allVehicleResult.Next))
                        {
                            if (nextVehicleResponse.IsSuccessStatusCode)
                            {
                                allVehicleResult = await nextVehicleResponse.Content.ReadAsAsync<AllVehiclesModel>();
                                vehicles.AddRange(allVehicleResult.VehicleList.Where<VehicleModel>(v => v.Cost != "unknown")); //exclude unknown cossts
                                
                            }
                            else
                            {
                                throw new Exception(response.ReasonPhrase);
                            }
                           

                        }
                    }
                    count = vehicles.Count.ToString();

                    var distinctManufacturers = vehicles.Select(v => v.Manufacturer).Distinct();


                    foreach (var manufacturer in distinctManufacturers)
                    {
                        var manuSet = vehicles.Where<VehicleModel>(v => v.Manufacturer == manufacturer);

                        var avgCost = manuSet.Average<VehicleModel>(v => Double.Parse(v.Cost));

                        var manuNum = manuSet.Count();

                        endResult.Details.Add(new VehicleStatsViewModel()
                        {
                            VehicleCount = manuNum,
                            AverageCost = avgCost,
                            ManufacturerName = manufacturer,
                        });

                    }

                    endResult.VehicleCount = int.Parse(count);
                    endResult.ManufacturerCount = endResult.Details.Count;


                    return endResult;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }

    }
}