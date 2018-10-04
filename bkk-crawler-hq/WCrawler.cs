using bkk_crawler_hq.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bkk_crawler_hq
{
    /// <summary>
    /// The Crawler is based on OpenWeatherMap 
    /// </summary>
    class WCrawler
    {
        /// <summary>
        /// The API key 
        /// </summary>
        private readonly string key = "4bd87942ef4b25b2f69f19a086353f49";
        /// <summary>
        /// The base URL of the API
        /// </summary>
        private readonly string base_url = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appId={2}&units=metric";


        public async Task<Weather> getWeatherByGeoTags(double lat, double lng)
        {
            string url = URLBuilder(lat, lng);
            Weather weather;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                weather = JsonConvert.DeserializeObject<Weather>(json);
            }
                                          
            return weather;
        }

        private string URLBuilder(double lat, double lng)
        {
            return string.Format(base_url, lat, lng, key).Replace(',','.');
        }


    }
}
