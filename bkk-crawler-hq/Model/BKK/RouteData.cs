using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace bkk_crawler_hq.Model.BKK
{
    public class RouteData
    {
        private string vehicleId;
        private string tripId;
        private string routeId;
        private string status;

        [JsonProperty("vehicleId")]
        public string VehicleId { get => vehicleId; set => vehicleId = value; }
        [JsonProperty("tripId")]
        public string TripId { get => tripId; set => tripId = value; }
        [JsonProperty("routeId")]
        public string RouteId { get => routeId; set => routeId = value; }
        [JsonProperty("status")]
        public string Status { get => status; set => status = value; }
    }
}
