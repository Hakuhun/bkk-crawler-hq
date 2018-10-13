using System;
using System.Runtime.Serialization;

namespace bkk_crawler_hq
{
    [Serializable]
    internal class BKKRouteNotFoundException : Exception
    {
        private string trip_id;
        private string veichle_id;
        private string route_id;

        public BKKRouteNotFoundException()
        {
        }

        public BKKRouteNotFoundException(string route_id, string trip_id, string veichle_id)
        {
            this.trip_id = trip_id;
            this.veichle_id = veichle_id;
        }

        public override string Message { get => string.Format(
                "{0}:{1} VISZONYLATON {2}/{3} járat nem található", 
                DateTime.UnixEpoch.ToString(), route_id, trip_id, veichle_id
            ); }

        public string VeichleID { get => veichle_id; set => veichle_id = value; }
        public string TripID { get => trip_id; set => trip_id = value; }
    }
}