using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace bkk_crawler_hq.Model
{
    public class Location
    {
        private float lat;
        private float lng;

        [JsonProperty("lat")]
        public float Lat
        {
            get => lat;
            set => lat = value;
        }

        [JsonProperty("lon")]
        public float Lng
        {
            get => lng;
            set => lng = value;
        }
    }
}
