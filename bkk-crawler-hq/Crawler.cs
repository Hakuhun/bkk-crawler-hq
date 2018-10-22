using System;
using System.Collections.Generic;
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
        private List<Trip> trips;
        private List<Weather> weathers;
        private static object blockobject = new object();
        private List<SimpleTripData> tripDatas = new List<SimpleTripData>();
        private List<SimpleWeatherData> weatherDatas = new List<SimpleWeatherData>();
        private List<Task> tasksToDo = new List<Task>();

        public Crawler()
        {
            this.bkk = new BKKCrawler();
            this.w = new WCrawler();

            this.weathers = new List<Weather>();
            this.trips = new List<Trip>();

        }

        public void getData()
        {
            foreach (RouteData route in this.bkk.AllRoutes)
            {
                Task task = new Task(() => { Crawl(route); }, TaskCreationOptions.LongRunning);
                tasksToDo.Add(task);
            }

            //Task.WaitAll(tasksToDo.ToArray(), CancellationToken.None);

            foreach (Task task in tasksToDo)
            {
                task.Start();
            }

            //Task.WhenAll(new Task(()=>SerializeData())).Start();

            //SerializeData();
        }

        public void SerializeData()
        {
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
            foreach (var trip in trips)
            {
                ParquetConvert.Serialize(trip.Stops, "stops.parquet");
            }
        }

        async void Crawl(RouteData route)
        {
            try
            {
                Trip trip = await bkk.getDetailedTripData(route);
                trips.Add(trip);
                Console.WriteLine(trip.Veichle.Model);
                lock (blockobject)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(trip.RouteID + "sikeresen olvasva");
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

