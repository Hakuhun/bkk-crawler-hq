using System;
using System.Runtime.Serialization;
using bkk_crawler_hq.Model.BKK;

namespace bkk_crawler_hq
{
    [Serializable]
    internal class BKKExcepton : Exception
    {
        private string trip_id;
        private string veichle_id;
        private string route_id;

        public BKKExcepton(string route_id, string trip_id, string veichle_id, string url = "")
        {
            this.route_id = route_id;
            this.trip_id = trip_id;
            this.veichle_id = veichle_id;
        }

        public BKKExcepton(RouteData rd)
        {
            this.route_id = rd.RouteId;
            this.trip_id = rd.TripId;
            this.veichle_id = rd.VehicleId;
        }

        public override string Message
        {
            get => string.Format("{0}//{1} : {2} @ {3}", route_id,DateTime.Now,veichle_id,trip_id);
        }

        public string VeichleID { get => veichle_id; set => veichle_id = value; }
        public string TripID { get => trip_id; set => trip_id = value; }

        public string RouteId
        {
            get { return route_id; }
            set { route_id = value; }
        }
    }
}