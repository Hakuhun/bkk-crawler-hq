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

        private object Valami(string route_id)
        {
            return null;
        }

        public async Task<Trip> getDetailedTripData(string trip_id, string veichle_id)
        {
            string url = this.URLBuilder(trip_id, veichle_id);
            Debug.Print(url);
            Trip trip;
            using (var httpClient = new HttpClient())
            {
                var json_text = await httpClient.GetStringAsync(url);
                
                JObject json = JObject.Parse(json_text);
                var asd = json.GetValue("code").Value<string>();
                
                if (asd != "404")
                {
                    var filtered_json = json.GetValue("data").SelectToken("entry");
                    trip = filtered_json.ToObject<Trip>();
                    return trip;
                }
                else
                    throw new BKKRouteNotFoundException("BKK_9790", trip_id, veichle_id);
            }
        }

        private string URLBuilder(string trip_id, string veichle_id)
        {
            return string.Format("{0}/trip-details.json?key={1}&version={2}&appVersion={3}&tripId={4}&vehicleId={5}", base_url, key, version, appVersion, trip_id, veichle_id);
        }

    }
}
