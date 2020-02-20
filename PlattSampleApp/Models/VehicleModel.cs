using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace PlattSampleApp.Models
{
    public class VehicleModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("vehicle_class")]
        public string VehicleClass { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("cost_in_credits")]
        public string Cost { get; set; }

        [JsonProperty("length")]
        public string Length { get; set; }

        [JsonProperty("crew")]
        public string Crew { get; set; }

        [JsonProperty("passengers")]
        public string Passengers { get; set; }

        [JsonProperty("max_atmosphering_speed")]
        public string MaxSpeed { get; set; }

        [JsonProperty("cargo_capacity")]
        public string CargoCapacity { get; set; }

        [JsonProperty("consumables")]
        public string ConsumableTime { get; set; }

        [JsonProperty("films")]
        public List<string> Films { get; set; }

        [JsonProperty("pilots")]
        public List<string> Pilots { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }



    }
}