using System;
using System.Runtime.Serialization;

namespace bkk_crawler_hq
{
    [Serializable]
    internal class BKKExcepton : Exception
    {
        private string trip_id;
        private string veichle_id;
        private string route_id;

        Logger.Logger logger = new Logger.Logger();

        public BKKExcepton(string route_id, string trip_id, string veichle_id, string url = "")
        {
            this.trip_id = trip_id;
            this.veichle_id = veichle_id;
            logger.Exception = this;
            logger.RouteId = route_id;
            logger.Url = url;
            logger.Write();
        }

        public override string Message
        {
            get => logger.Message;
        }

        public string VeichleID { get => veichle_id; set => veichle_id = value; }
        public string TripID { get => trip_id; set => trip_id = value; }
    }
}