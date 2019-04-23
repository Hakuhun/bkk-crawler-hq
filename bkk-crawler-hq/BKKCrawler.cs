using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

        private List<RouteData> allRoutes = new List<RouteData>();

        public BKKCrawler()
        {
            InitRoutes();
        }

        private static readonly List<string> frequent_routes = new List<string>()
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
            //"BKK_MP533",//M3 pótló
            //"BKK_MP53",//M3 pótló
            //"BKK_MP536",//M3 pótló
            //"BKK_MP531",//M3 pótló
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
            "BKK_0090",//9-es busz
            "BKK_9790", //979
            "BKK_9791",//979A
            "BKK_9080", //908
            "BKK_9070",
            "BKK_9140",
            "BKK_9230"

        };

        public List<RouteData> AllRoutes
        {
            get { return allRoutes; }
        }

        protected void InitRoutes()
        {
            foreach (string route in frequent_routes)
            {
                List<RouteData> routes = getRouteDataByRoute(route);
                if (routes != null)
                {
                    allRoutes.AddRange(routes);
                }
            }
        }

        //public async Task<List<RouteData>> getRouteDataByRoute(string route_id)
        public List<RouteData> getRouteDataByRoute(string route_id)
        {
            List<RouteData> routes = null;
            var url = this.URLBuilder(route_id);
            Debug.Print("Route: "+ url + Environment.NewLine);
            //using (var httpClient = new HttpClient())
            using (var httpClient = new WebClient())
            {
                var jsonText = httpClient.DownloadString(url);
                //var jsonText = httpClient.GetString(url);
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

        //public async Task<Trip> getDetailedTripData(RouteData route)
        public Trip getDetailedTripData(RouteData route)
        {
            string url = this.URLBuilder(route);
            Debug.Print(url + "\n");
            Trip trip;

            //using (var httpClient = new HttpClient())
            using (var httpClient = new WebClient())
            {
                //var jsonText = await httpClient.GetStringAsync(url);
                var jsonText = httpClient.DownloadString(url);

                JObject json = JObject.Parse(jsonText);
                var conditionCode = json.GetValue("code").Value<string>();
                var currenttime = long.Parse(json.GetValue("currentTime").Value<string>());

                if (conditionCode == "200")
                {
                    var filteredJson = json.GetValue("data").SelectToken("entry");
                    trip = filteredJson.ToObject<Trip>();
                    trip.RouteID = route.RouteId;
                    trip.CurrentTime = currenttime;

                    for (int i = 0; i < trip.Stops.Count; i++)
                    {
                        trip.Stops[i].CurrentTime = (int) trip.CurrentTime / 1000;
                        trip.Stops[i].RouteId = trip.RouteID;
                        trip.Stops[i].TripId = trip.Veichle.TripID;
                        trip.Stops[i].VeichleId = trip.Veichle.VeichleID;
                    }

                    return trip;
                }
                else
                    throw new BKKExcepton(route);
            }
        }

        private string URLBuilder(string route_id)
        {
            return string.Format("{0}vehicles-for-route.json?key={1}&version={2}&appVersion={3}&routeId={4}", base_url, key, version, appVersion, route_id);
        }
        private string URLBuilder(RouteData rd)
        {
            return string.Format("{0}trip-details.json?key={1}&version={2}&appVersion={3}&tripId={4}&vehicleId={5}", base_url, key, version, appVersion, rd.TripId, rd.VehicleId);
        }
    }
}
