using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bkk_crawler_hq
{
    /// <summary>
    /// 
    /// </summary>
    class BKKCrawler
    {
        /// <summary>
        /// The API key 
        /// </summary>
        private readonly string key = "bkk-web";
        /// <summary>
        /// The base URL of the API
        /// </summary>
        private readonly string base_url = "http://futar.bkk.hu/bkk-utvonaltervezo-api/ws/otp/api/where/";
        /// <summary>
        /// The used application api, using the apiary's one.
        /// TODO: May get an app key for this app
        /// </summary>
        private readonly string appVersion = "apiary-1.0";
        /// <summary>
        /// API Version
        /// </summary>
        private readonly string version = "3";

        protected List<RouteData> allroutes = new List<RouteData>();

        public BKKCrawler()
        {
            InitRoutes();
        }

        private readonly List<string> frequent_routes = new List<string>()
        {
            "BKK_3010",//1-es villamos
            "BKK_3020",//2-es villamos
            "BKK_3040",//4-es villamos
            "BKK_3046",//6-os villamos
            "BKK_VP06",//6-os villamospótló
            "BKK_3170",//17-es villamos
            "BKK_3190",//19-es villamos
            "BKK_3410",//41-es villamos
            "BKK_OPM1",//M1
            "BKK_OPM2",//M2
            "BKK_OPM3",//M3
            "BKK_OPM4",//M4
            "BKK_MP533",//M3 pótló
            "BKK_MP53",//M3 pótló
            "BKK_MP536",//M3 pótló
            "BKK_MP531",//M3 pótló
            "BKK_6470",//H5-> Békásmegyer
            "BKK_HK64",//H5 pótló
            "BKK_OPH5",//H5 pótló
            "BKK_6230",//H6 -> Dunaharaszti
            "BKK_6210",//H6 -> Tököl
            "BKK_6200",//H6 -> Ráckeve
            "BKK_OPH6",//H6 pótló
            "BKK_6300",//H7 -> CSEPEL <3 
            "BKK_OPH7",//H7 pótló
            "BKK_6100",//H8 -> Gödöllő
            "BKK_6130",//H8 -> Cinkota
            "BKK_OPH8",//H8 pótló
            "BKK_0050",//5-ös busz
            "BKK_0070",//7-es busz
            "BKK_0075",//7E-es busz
            "BKK_0085",//8E
            "BKK_0090"//9-es busz

        };

        protected void InitRoutes()
        {
            foreach (string route in frequent_routes)
            {
                try
                {
                    List<RouteData> routes = getRouteDataByRoute(route).Result;
                    if (routes != null)
                    {
                        allroutes.AddRange(routes);
                    }
                }
                catch (BKKExcepton e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                
            }
        } 

        public List<Trip> getTrips()
        {
            List<Trip> local = new List<Trip>();
            foreach (RouteData route in allroutes)
            {
                Trip trip = getDetailedTripData(route.RouteId, route.TripId, route.VehicleId).Result;
                local.Add(trip);
            }

            return local;
        }

        protected async Task<List<RouteData>> getRouteDataByRoute(string route_id)
        {
            List<RouteData> routes = null;
            var url = this.URLBuilder(route_id);
            using (var httpClient = new HttpClient())
            {
                var jsonText = await httpClient.GetStringAsync(url);
                JObject json = JObject.Parse(jsonText);
                var conditionCode = json.GetValue("code").Value<string>();
                if (conditionCode == "200")
                {
                    var filteredJson = json.GetValue("data").SelectToken("list");
                    routes = filteredJson.ToObject<List<RouteData>>();
                }
            }

            return routes;
        }

        protected async Task<Trip> getDetailedTripData(string route_id, string trip_id, string veichle_id)
        {
            string url = this.URLBuilder("",trip_id,veichle_id);
            Debug.Print(url);
            Trip trip;
            using (var httpClient = new HttpClient())
            {
                var jsonText = await httpClient.GetStringAsync(url);
                
                JObject json = JObject.Parse(jsonText);
                var conditionCode = json.GetValue("code").Value<string>();
                
                if (conditionCode == "200")
                {
                    var filteredJson = json.GetValue("data").SelectToken("entry");
                    trip = filteredJson.ToObject<Trip>();
                    return trip;
                }
                else
                    throw new BKKExcepton(route_id, trip_id, veichle_id);
            }
        }

        private string URLBuilder(string route_id = "", string trip_id = "", string veichle_id = "")
        {
            if (route_id != "" && trip_id == "")
            {
                return string.Format("{0}/vehicles-for-route.json?key={1}&version={2}&appVersion={3}&routeId={4}", base_url, key, version, appVersion, route_id);
            }
            else
            {
                return string.Format("{0}/trip-details.json?key={1}&version={2}&appVersion={3}&tripId={4}&vehicleId={5}", base_url, key, version, appVersion, trip_id, veichle_id);
            }
        }

    }
}
