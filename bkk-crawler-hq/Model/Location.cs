using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace bkk_crawler_hq.Model
{
    public class Location
    {
        private double lat;
        private double lng;

        [JsonProperty("lat")]
        public double Lat
        {
            get => lat;
            set => lat = value;
        }

        [JsonProperty("lon")]
        public double Lng
        {
            get => lng;
            set => lng = value;
        }
    }
}
