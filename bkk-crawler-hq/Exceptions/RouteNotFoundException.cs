using System;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json.Linq;

namespace bkk_crawler_hq.Exceptions
{
    public class RouteNotFoundException : BKKException 
    {
        public RouteNotFoundException(string routeId) : base(routeId, "N/A","N/A")
        {
            
        }
        
        public RouteNotFoundException(string route_id, string trip_id, string veichle_id, string url = "") : base(route_id, trip_id, veichle_id, url)
        {
            
        }

        public RouteNotFoundException(RouteData rd) : base(rd)
        {
        }

        public override string Message
        {
            get
            {
                JObject json = JObject.FromObject(new {
                    status = "ROUTE NOT EXISTS",
                    at = DateTime.Now,
                    on = "Initalizing routes",
                    routeId = RouteId
                });

                return json.ToString();
            }
        }
    }
}