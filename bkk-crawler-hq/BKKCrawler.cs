using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.LocalData;
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
        private readonly string base_url = "https://futar.bkk.hu/api/query/v1/ws/otp/api/where/";
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

        public List<RouteData> AllRoutes
        {
            get { return allRoutes; }
        }

        protected void InitRoutes()
        {
            var db = new BKKinfoContext();
            foreach (BKKInfo bkkinfo in db.Bkkinfo)
            {
                List<RouteData> routes = getRouteDataByRoute(bkkinfo.Code);
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

            if (route.TripId == null || route.TripId == String.Empty || route.VehicleId == null || route.VehicleId == String.Empty)
                throw new MissingRouteDataException(route);

            //using (var httpClient = new HttpClient())
            using (var httpClient = new WebClient())
            {
                //var jsonText = await httpClient.GetStringAsync(url);
                var jsonText = httpClient.DownloadString(url);

                JObject json = JObject.Parse(jsonText);
                var conditionCode = json.GetValue("code").Value<string>();
                var currenttime = //DateTimeOffset.Now.ToUnixTimeSeconds();
                (long) long.Parse(json.GetValue("currentTime").Value<string>()) ;


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
                {
                    throw new BKKException(route);
                }
            }
        }

        private string URLBuilder(string route_id)
        {
            return string.Format("{0}vehicles-for-route.json?key={1}&version={2}&appVersion={3}&routeId={4}&includeReferences=false&related=false", base_url, key, version, appVersion, route_id);
        }
        private string URLBuilder(RouteData rd)
        {
            return string.Format("{0}trip-details.json?key={1}&version={2}&appVersion={3}&tripId={4}&vehicleId={5}&includeReferences=false&related=false", base_url, key, version, appVersion, rd.TripId, rd.VehicleId);
        }
    }
}
