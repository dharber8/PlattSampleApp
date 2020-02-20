using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PlattSampleApp
{
    public static class ApiHelper 
    {
        public static HttpClient ApiClient { get; set; }
        public static string baseClass { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();

            ApiClient.BaseAddress = new Uri(@"https://swapi.co/api/");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}