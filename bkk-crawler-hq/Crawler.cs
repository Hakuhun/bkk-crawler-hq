using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Model.Parquet;
using Parquet;
using Parquet.Data;

namespace bkk_crawler_hq
{
    class Crawler
    {
        private BKKCrawler bkk;
        private WCrawler w;
        //private List<Trip> trips;
        //private List<Weather> weathers;
        private static object blockobject = new object();
        private ConcurrentBag<SimpleTripData> tripDatas = new ConcurrentBag<SimpleTripData>();
        private ConcurrentBag<SimpleWeatherData> weatherDatas = new ConcurrentBag<SimpleWeatherData>();
        private ConcurrentBag<Task> tasksToDo = new ConcurrentBag<Task>();
        private ConcurrentBag<StopData> stopDatas = new ConcurrentBag<StopData>();

        public Crawler()
        {
            this.bkk = new BKKCrawler();
            this.w = new WCrawler();

            //this.weathers = new List<Weather>();
            //this.trips = new List<Trip>();

        }

        public void getData()
        {
            foreach (RouteData route in this.bkk.AllRoutes)
            {
                Task task = new Task(() => { Crawl(route); }, TaskCreationOptions.LongRunning);
                tasksToDo.Add(task);
            }
            
            foreach (Task task in tasksToDo)
            {
                task.Start();
            }

            Task.WaitAll(tasksToDo.ToArray(), CancellationToken.None);

            SerializeData();
        }

        public void SerializeData()
        {
            tripDatas.OrderBy(x => x.RouteID).OrderBy(x => x.VeichleID);
            SerializeTrips();
            //SerializeWeather();
            SerializeStops();
        }

        private void SerializeTrips()
        {
            Schema schema = ParquetConvert.Serialize(tripDatas, "trips.parquet");
        }

        private void SerializeWeather()
        {
            Schema schema = ParquetConvert.Serialize(weatherDatas, "weathers.parquet");
        }

        private void SerializeStops()
        {
            //foreach (var trip in trips)
            //{
            //    ParquetConvert.Serialize(trip.Stops, "stops.parquet");
            //}
            ParquetConvert.Serialize(stopDatas, "stops.parquet");
        }

        async void Crawl(RouteData route)
        {
            try
            {
                Trip trip = await bkk.getDetailedTripData(route);
                //trips.Add(trip);
                foreach (var stop in trip.Stops)
                {
                    stopDatas.Add(stop);
                }

                lock (blockobject)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(trip.RouteID + " sikeresen olvasva");
                }

                tripDatas.Add(trip.getparquetFormat());

                //Weather weather = await w.getWeatherByGeoTags(trip.Veichle.Location);
                //weather.CurrentTime = trip.CurrentTime;
                //weathers.Add(weather);
                //lock (blockobject)
                //{
                //    Console.ForegroundColor = ConsoleColor.Blue;
                //    Console.WriteLine(trip.RouteID + "-hoz tartozó időjárás adat sikeresen olvasva");
                //}

                //weatherDatas.Add(weather.getParquetFormat());


            }
            catch (NotInTransitException nit)
            {
                lock (blockobject)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("HIBA: " + nit.Message + Environment.NewLine);
                }
            }
            catch (BKKExcepton e)
            {
                lock (blockobject)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("HIBA: " + e.Message + Environment.NewLine);
                }
            }
        }
    }
}

