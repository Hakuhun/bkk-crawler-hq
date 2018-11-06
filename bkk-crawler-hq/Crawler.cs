﻿using bkk_crawler_hq.Exceptions;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Model.Parquet;
using Parquet;
using Parquet.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bkk_crawler_hq
{
    class Crawler
    {
        private BKKCrawler bkk;
        private WCrawler w;
        private static object blockobject = new object();
        private ConcurrentBag<ISimpleDataModel> tripDatas = new ConcurrentBag<ISimpleDataModel>();
        private ConcurrentBag<ISimpleDataModel> weatherDatas = new ConcurrentBag<ISimpleDataModel>();
        private List<Task> tasksToDo = new List<Task>();
        private ConcurrentBag<StopData> stopDatas = new ConcurrentBag<StopData>();

        private readonly string weather_path = "D:/BKK/weather/", trip_path = "D:/BKK/trip/", stop_path = "D:/BKK/stop/";

        public Crawler()
        {
            this.bkk = new BKKCrawler();
            this.w = new WCrawler();
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
            Serializator.SerializeCollectionToCSV(tripDatas, trip_path + "trips.csv");
            Serializator.SerializeCollectionToCSV(weatherDatas, weather_path + "weathers.csv");
            Serializator.SerializeCollectionToCSV(stopDatas, stop_path + "stops.csv");
            Serializator.SerializeCollectionToJSON(tripDatas, trip_path + "trips.json");
            Serializator.SerializeCollectionToJSON(weatherDatas, weather_path + "weathers.json");
            Serializator.SerializeCollectionToJSON(stopDatas, stop_path + "stops.json");
        }

        async void Crawl(RouteData route)
        {
            try
            {
                Trip trip = await bkk.getDetailedTripData(route).ConfigureAwait(true);
                foreach (var stop in trip.Stops)
                {
                    stopDatas.Add(stop);
                }

                lock (blockobject)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(trip.RouteID + " sikeresen olvasva");
                }

                tripDatas.Add(trip.getSerializableFormat());

                Weather weather = await w.getWeatherByGeoTags(trip.Veichle.Location).ConfigureAwait(true);
                weather.CurrentTime = trip.CurrentTime;

                lock (blockobject)
                {
                    if (weather != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(trip.RouteID + "-hoz tartozó időjárás adat sikeresen olvasva");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("HIBA: " + trip.RouteID +"-hoz tartozó időjárásadat nem lett olvasva." + Environment.NewLine);
                    }
                }

                SimpleWeatherData swd = weather.getParquetFormat();
                swd.RouteID = trip.RouteID;
                swd.TripID = trip.Veichle.TripID;
                weatherDatas.Add(swd);
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

