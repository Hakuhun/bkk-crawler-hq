using Newtonsoft.Json;

namespace bkk_crawler_hq.Model.BKK
{
    public partial class VeichleData
    {
        private string trip_id;
        private string veichle_id;
        private string model;
        private string status;
        private Location location;
        private string veichleType;

        [JsonProperty("tripId")]
        public string TripID
        {
            get => trip_id;
            set => trip_id = value;
        }

        [JsonProperty("vehicleId")]
        public string VeichleID
        {
            get => veichle_id;
            set => veichle_id = value;
        }

        [JsonProperty("model")]
        public string Model
        {
            get => model;
            set => model = value;
        }

        [JsonProperty("status")]
        public string Status
        {
            get => status;
            set => status = value;
        }

        [JsonProperty("location")]
        public Location Location
        {
            get => location;
            set => location = value;
        }

        [JsonProperty("vehicleRouteType")]  
        public string VeichleType
        {
            get => veichleType;
            set => veichleType = value;
        }
    }
}
