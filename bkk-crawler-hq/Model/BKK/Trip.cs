using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace bkk_crawler_hq.Model.BKK
{
    public class Trip
    {
        private string route_id;
        private VeichleData veichle;
        private List<StopData> stops;

        public string RouteID
        {
            get => route_id;
            set => route_id = value;
        }

        [JsonProperty("vehicle")]
        public VeichleData Veichle
        {
            get => veichle;
            set => veichle = value;
        }

        [JsonProperty("stopTimes")]
        public List<StopData> Stops
        {
            get => stops;
            set => stops = value;
        }

    }
}
