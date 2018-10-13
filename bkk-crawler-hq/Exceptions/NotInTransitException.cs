using bkk_crawler_hq.Model.BKK;

namespace bkk_crawler_hq.Exceptions
{
    class NotInTransitException : BKKExcepton
    {
        public NotInTransitException(string route_id, string trip_id, string veichle_id, string url = "") : base(route_id, trip_id, veichle_id, url)
        {
        }

        public NotInTransitException(RouteData rd) : base(rd)
        {
        }
    }
}
