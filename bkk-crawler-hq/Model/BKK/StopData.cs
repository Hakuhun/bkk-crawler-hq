using Newtonsoft.Json;

namespace bkk_crawler_hq.Model.BKK
{
    public class StopData
    {
        private string stopID;
        private int stop_squence;
        private long departureTime;
        private long predictedDepartureTime;
        private long arrivalTime;
        private long predictedArrivalTime;
        private string routeId, tripId, veichleId;

        [JsonProperty("stopId")]
        public string StopId
        {
            get => stopID;
            set => stopID = value;
        }

        [JsonProperty("stopSequence")]
        public int StopSquence
        {
            get => stop_squence;
            set => stop_squence = value;
        }

        [JsonProperty("departureTime")]
        public long DepartureTime
        {
            get => departureTime;
            set => departureTime = value;
        }

        [JsonProperty("predictedDepartureTime")]
        public long PredictedDepartureTime
        {
            get => predictedDepartureTime;
            set => predictedDepartureTime = value;
        }

        [JsonProperty("arrivalTime")]
        public long ArrivalTime
        {
            get => arrivalTime;
            set => arrivalTime = value;
        }

        [JsonProperty("predictedArrivalTime")]
        public long PredictedArrivalTime
        {
            get => predictedArrivalTime;
            set => predictedArrivalTime = value;
        }

        public string RouteId
        {
            get { return routeId; }
            set { routeId = value; }
        }

        public string TripId
        {
            get { return tripId; }
            set { tripId = value; }
        }

        public string VeichleId
        {
            get { return veichleId; }
            set { veichleId = value; }
        }
    }
}
