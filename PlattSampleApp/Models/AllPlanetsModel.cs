using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace PlattSampleApp.Models
{
    public class AllPlanetsModel
    {
        [JsonProperty("count")]
        public string Count { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("results")]
        public List<PlanetModel> PlanetList {get; set; }

    }
}