using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;

namespace bkk_crawler_hq
{
    class Crawler
    {
        private BKKCrawler bkk;
        private WCrawler w;
        private List<Trip> trips;
        private List<Weather> weathers;

        public Crawler()
        {
            this.bkk =new BKKCrawler();
            this.w = new WCrawler();

            this.weathers = new List<Weather>();
            this.trips = new List<Trip>();
        }

        public void getData()
        {
            foreach (RouteData route in this.bkk.AllRoutes)
            {
                new Task(async () =>
                    {
                        try
                        {
                            Trip trip = await bkk.getDetailedTripData(route);
                            trips.Add(trip);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(trip.RouteID + "sikeresen olvasva");
                            Weather weather = await w.getWeatherByGeoTags(trip.Veichle.Location);
                            weathers.Add(weather);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(trip.RouteID + "-hoz tartozó időjárás adat sikeresen olvasva");
                        }
                        catch (NotInTransitException nit)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(nit.Message + Environment.NewLine);
                        }
                        catch (BKKExcepton e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(e.Message + Environment.NewLine);
                        }
                    }, TaskCreationOptions.LongRunning).Start();
            }
        }
    }
}
