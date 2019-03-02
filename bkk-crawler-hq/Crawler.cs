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
        private WCrawler w;
        private static object blockobject = new object();
        private ConcurrentBag<ISimpleDataModel> tripDatas = new ConcurrentBag<ISimpleDataModel>();
        private ConcurrentBag<ISimpleDataModel> weatherDatas = new ConcurrentBag<ISimpleDataModel>();
        private ConcurrentBag<ISimpleDataModel> collectedData = new ConcurrentBag<ISimpleDataModel>();
        private List<Task> tasksToDo = new List<Task>();
        private ConcurrentBag<StopData> stopDatas = new ConcurrentBag<StopData>();

        //private readonly string weather_path = "D:/BKK/weather/", trip_path = "D:/BKK/trip/", stop_path = "D:/BKK/stop/";

        private readonly string weather_path = String.Empty, trip_path = String.Empty, stop_path = String.Empty;

        public string Message => string.Format("{0}. viszonylaton, {1}. járat és {2}. megálló adata begyűjve.",
            bkk.AllRoutes.Count, tripDatas.Count, stopDatas.Count);

        public Crawler()
        {
            this.bkk = new BKKCrawler();
            this.w = new WCrawler();
        }

        public void getDataParallel()
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
        }

        public void GetDataSequential()
        {
            BKKCrawler bkkc = new BKKCrawler();
            WCrawler wc = new WCrawler();
            foreach (var route in bkkc.AllRoutes)
            {
                Trip local_trip = null;
                try
                {
                    local_trip = bkkc.getDetailedTripData(route);
                    tripDatas.Add(local_trip.getSerializableFormat());
                    foreach (StopData stop in local_trip.Stops)
                    {
                        stopDatas.Add(stop);
                    }
                    Weather local_weather = wc.getWeatherByGeoTags(local_trip.Veichle.Location);

                    weatherDatas.Add(local_weather.getSerializableFormat()); ;
                }
                catch (BKKExcepton)
                {
                    Console.WriteLine("HIBA");
                }

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

        async void Crawl(RouteData route)
        {
            try
            {
                //Trip trip = await bkk.getDetailedTripData(route);
                Trip trip = bkk.getDetailedTripData(route);
                foreach (var stop in trip.Stops)
                {
                    stopDatas.Add(stop);
                }

                lock (blockobject)
                {
                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.WriteLine(trip.RouteID + " sikeresen olvasva");
                }

                tripDatas.Add(trip.getSerializableFormat());

                //Weather weather = await w.getWeatherByGeoTags(trip.Veichle.Location);
                Weather weather = w.getWeatherByGeoTags(trip.Veichle.Location);
                weather.CurrentTime = trip.CurrentTime;

                lock (blockobject)
                {
                    if (weather != null)
                    {
                        //Console.ForegroundColor = ConsoleColor.Blue;
                        //Console.WriteLine(trip.RouteID + "-hoz tartozó időjárás adat sikeresen olvasva");
                    }
                    else
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        ///Console.WriteLine("HIBA: " + trip.RouteID +"-hoz tartozó időjárásadat nem lett olvasva." + Environment.NewLine);
                    }
                }

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

