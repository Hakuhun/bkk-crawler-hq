using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bkk_crawler_hq
{
    /// <summary>
    /// 
    /// </summary>
    class BkkCrawler
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

        public BkkCrawler()
        {
            //InitRoutes();
        }

        //public List<RouteData> AllRoutes => allRoutes;

        public List<RouteData> GetRouteBaseDatas()
        {
            var db = new bkkinfoContext();
            foreach (Bkkinfo bkkinfo in db.Bkkinfo)
            {
                List<RouteData> routes = null;
                try
                {
                    routes = GetRouteDataByRoute(bkkinfo.Code);
                    DateTime start = DateTime.Parse(bkkinfo.Start);
                    DateTime end = DateTime.Parse(bkkinfo.End);
                    if (end.Hour >= 0 && end.Hour <= 5)
                    {
                        end = end + TimeSpan.FromDays(1);
                    }
                    
                    if (bkkinfo.RouteType != "NONSTOP" && BetweenTwoTimeHourMinute(start, end))
                    {
                        allRoutes.AddRange(routes);
                    }else if (bkkinfo.RouteType == "NONSTOP")
                    {
                        allRoutes.AddRange(routes);
                    }
                }
                catch (RouteNotFoundException e)
                {
                    Console.WriteLine(e);
                }
            }

            return allRoutes;
        }

        //public async Task<List<RouteData>> getRouxteDataByRoute(string route_id)
        private List<RouteData> GetRouteDataByRoute(string routeId)
        {
            List<RouteData> routes = null;
            var url = this.UrlBuilder(routeId);
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
                else
                {
                    throw new RouteNotFoundException(routeId);
                }
            }

            return routes;
        }

        //public async Task<Trip> getDetailedTripData(RouteData route)
        public Trip GetDetailedTripData(RouteData route)
        {
            string url = this.UrlBuilder(route);
            Debug.Print(url + "\n");
            Trip trip;

            if (string.IsNullOrEmpty(route.TripId) || route.VehicleId == null || route.VehicleId == String.Empty)
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

        private string UrlBuilder(string routeId)
        {
            return
                $"{base_url}vehicles-for-route.json?key={key}&version={version}&appVersion={appVersion}&routeId={routeId}&includeReferences=false&related=false";
        }
        private string UrlBuilder(RouteData rd)
        {
            return
                $"{base_url}trip-details.json?key={key}&version={version}&appVersion={appVersion}&tripId={rd.TripId}&vehicleId={rd.VehicleId}&includeReferences=false&related=false";
        }

        private bool BetweenTwoTimeHourMinute(DateTime start, DateTime end)
        {
            DateTime now = DateTime.Now;
            var editedStart = (start - TimeSpan.FromHours(1));
            var editedEnd = end + (TimeSpan.FromHours(1));
            return  editedStart <= now && editedEnd >= end;
        }
    }
}
