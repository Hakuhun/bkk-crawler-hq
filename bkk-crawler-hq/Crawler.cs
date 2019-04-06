using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Model.Parquet;
using bkk_crawler_hq.Model.Serialization;
using Parquet;
using Parquet.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bkk_crawler_hq
{
    class Crawler
    {
        Stopwatch sw = new Stopwatch();

        private BKKCrawler bkk;

        private MapDetails MapDetails = MapDetails.getInstance();

        private static object blockobject = new object();
        private ConcurrentBag<ISimpleDataModel> tripDatas = new ConcurrentBag<ISimpleDataModel>();
        private ConcurrentBag<ISimpleDataModel> weatherDatas = new ConcurrentBag<ISimpleDataModel>();
        private ConcurrentBag<ISimpleDataModel> collectedData = new ConcurrentBag<ISimpleDataModel>();
        private List<Task> routeDownloaderTasks = new List<Task>();
        private ConcurrentBag<StopData> stopDatas = new ConcurrentBag<StopData>();

        //private readonly string weather_path = "D:/BKK/weather/", trip_path = "D:/BKK/trip/", stop_path = "D:/BKK/stop/";

        private readonly string weather_path = String.Empty, trip_path = String.Empty, stop_path = String.Empty;

        public string Message => string.Format("{0}. viszonylaton, {1}. járat és {2}. megálló adata begyűjve.",
            bkk.AllRoutes.Count, tripDatas.Count, stopDatas.Count);

        public Crawler()
        {
            this.bkk = new BKKCrawler();
        }

        public void DownloadWeatherDatas()
        {
            Task weatherDownloaderTask = new Task(() => MapDetails.DownloadWeatherData(), CancellationToken.None, TaskCreationOptions.LongRunning);
            weatherDownloaderTask.Start();
        }

        public void DownloadRouteDatas()
        {
            if (MapDetails != null)
            {
                foreach (RouteData route in this.bkk.AllRoutes)
                {
                    Task task = new Task(() => { Crawl(route); }, TaskCreationOptions.LongRunning);
                    routeDownloaderTasks.Add(task);
                    task.Start();
                }

                //foreach (Task task in routeDownloaderTasks)
                //{
                //    task.Start();
                //}

                Task.WaitAll(routeDownloaderTasks.ToArray(), CancellationToken.None);
            }
        }

        public void clearData()
        {
            tripDatas.Clear();
            stopDatas.Clear();
            weatherDatas.Clear();
        }

        public void SerializeData()
        {
            Serializator.SerializeCollectionToCSV(tripDatas, trip_path + "trips.csv");
            Console.WriteLine("Trip data frissítve");
            Serializator.SerializeCollectionToCSV(weatherDatas, weather_path + "weathers.csv");
            Console.WriteLine("Weather data frissítve");
            Serializator.SerializeCollectionToCSV(stopDatas, stop_path + "stops.csv");
            Console.WriteLine("Stop data frisítve");
            Serializator.SerializeCollectionToCSV(collectedData, "data.csv");
            Console.WriteLine("Összesített adat frissítve");
            //Serializator.SerializeCollectionToJSON(tripDatas, trip_path + "trips.json");
            //Serializator.SerializeCollectionToJSON(weatherDatas, weather_path + "weathers.json");
            //Serializator.SerializeCollectionToJSON(stopDatas, stop_path + "stops.json");
        }

        void Crawl(RouteData route)
        {
            try
            {
                Trip trip = bkk.getDetailedTripData(route);
                foreach (var stop in trip.Stops)
                {
                    stopDatas.Add(stop);
                }

                tripDatas.Add(trip.getSerializableFormat());

                Weather weather = MapDetails.GetWeatherByTrip(trip);

                SimpleWeatherData swd = weather.getSerializableFormat();
                swd.RouteID = trip.RouteID;
                swd.TripID = trip.Veichle.TripID;
                weatherDatas.Add(swd);
                collectedData.Add(new SimpleCollectedData()
                {
                    SWeather = swd,
                    STrip = (trip.getSerializableFormat() as SimpleTripData),
                    Stops = trip.Stops
                });

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

