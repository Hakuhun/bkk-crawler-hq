using System;
using System.Collections.Generic;
using System.Text;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bkk_crawler_hq.Exceptions
{
    class MissingRouteDataException : BKKException
    {
        public MissingRouteDataException(RouteData rd) : base(rd)
        {
        }

        public MissingRouteDataException(string route_id, string trip_id, string veichle_id, string url = "") : base(route_id, trip_id, veichle_id, url)
        {
        }

        public override string Message
        {
            get
            {
                List<String> missingDatas = new List<string>();

                List<JObject> existingDatas = new List<JObject>();

                if (RouteId == String.Empty || RouteId == null)
                {
                    missingDatas.Add("routeId");
                }
                else
                {
                    existingDatas.Add(JObject.FromObject(new
                    {
                        routeId = RouteId
                    }));
                }
                if (TripID == String.Empty || TripID == null)
                {
                    missingDatas.Add("tripId");
                }
                else
                {
                    existingDatas.Add(JObject.FromObject(new
                    {
                        tripId = TripID
                    }));
                }
                if (VeichleID == String.Empty ||VeichleID == null)
                {
                    missingDatas.Add("veichleId");
                }
                else
                {
                    existingDatas.Add(JObject.FromObject(new
                    {
                        veichleId = VeichleID
                    }));
                }


                JObject json = JObject.FromObject(new {
                    status = "MISSING_DATA",
                    at = DateTime.Now,
                    on = "downloading detailed route data",
                    missingParts = missingDatas,
                    existingParts = existingDatas
                });

                return json.ToString();
            }
        }
    }
}
