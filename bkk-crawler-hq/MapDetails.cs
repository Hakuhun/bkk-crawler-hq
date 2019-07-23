using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Text;

namespace bkk_crawler_hq
{
    public class MapDetails
    {
        private static MapDetails instance;

        private const int X = 10, Y = 6;
        private const string weatherpropertypath = "weatherproperties.json";
        private WCrawler w = new WCrawler();

        private static readonly Location LowerLeft = new Location(47.1523107, 18.8460594);
        private static readonly Location UpperRight = new Location(47.6837053, 19.391385500000002);

        private double DiffLat => Math.Abs(UpperRight.Lat - LowerLeft.Lat) / X;
        private double DiffLng => Math.Abs(UpperRight.Lng - LowerLeft.Lng) / Y;

        public DateTime lastSaved { get; set; }

        public ConcurrentBag<Chunk> Chunks { get; set; }

        private MapDetails()
        {
            InitMapDetails();
        }

        private void InitMapDetails()
        {
            if (!File.Exists(weatherpropertypath))
            {
                Chunks = CalculateChunks();
            }
        }

        public static MapDetails GetInstance()
        {
            if (instance == null)
            {
                instance = Deserialize();
            }
            instance.DownloadWeatherData();
            return instance;
        }

        public Weather GetWeatherByTrip(Trip trip)
        {
            Weather w = MinimalDistance(trip).Weather;
            w.Latitude = trip.Veichle.Location.Lat;
            w.Longitude = trip.Veichle.Location.Lng;
            w.CurrentTime = trip.CurrentTime;
            return w;
        }

        private ConcurrentBag<Chunk> CalculateChunks()
        {
            ConcurrentBag<Chunk> local = new ConcurrentBag<Chunk>();
            for (int i = 0; i < X; i++)
            {
                double lat = LowerLeft.Lat + (i * DiffLat);
                for (int j = 0; j < Y; j++)
                {
                    double lng = LowerLeft.Lng + (j * DiffLng);
                    Location loc = new Location(lat, lng);
                    local.Add(new Chunk() { Center = loc });
                }
            }
            return local;
        }

        public Chunk MinimalDistance(Trip trip)
        {
            Dictionary<double, Chunk> distances = new Dictionary<double, Chunk>();
            foreach (Chunk chunk in Chunks)
            {
                distances[Distance(chunk.Center, trip.Veichle.Location)] = chunk;
            }

            double min = int.MaxValue;

            foreach (KeyValuePair<double, Chunk> data in distances)
            {
                if (min > data.Key)
                {
                    min = data.Key;
                }
            }
            return distances[min];
        }

        private double Distance(Location chunk, Location veichle)
        {
            GeoCoordinate pin1 = new GeoCoordinate(chunk.Lat, chunk.Lng);
            GeoCoordinate pin2 = new GeoCoordinate(veichle.Lat, veichle.Lng);

            return pin1.GetDistanceTo(pin2);
        }

        private void Serialize()
        {
            string output = JsonConvert.SerializeObject(this);
            //Console.WriteLine(output);
            StreamWriter sw = new StreamWriter(weatherpropertypath,false, Encoding.UTF8);
            sw.Write(output);
            sw.Close();
        }

        private static MapDetails Deserialize()
        {
            if (File.Exists(weatherpropertypath))
            {
                string input = File.ReadAllText(weatherpropertypath);
                var a = JsonConvert.DeserializeObject<MapDetails>(input);
                return a;
            }
            else
            {
                return new MapDetails();
            }
        }

        public void DownloadWeatherData()
        {
            TimeSpan diff = (DateTime.Now - this.lastSaved);
            if (this.lastSaved == null ||  diff.Hours >= TimeSpan.FromHours(2).Hours)
            {
                Console.WriteLine("Időjárásadatok frissítve");
                lastSaved = DateTime.Now;
                foreach (Chunk chunk in Chunks)
                {
                    chunk.Weather = w.getWeatherByGeoTags(chunk.Center);
                }
                Serialize();
            }
        }
    }
}
