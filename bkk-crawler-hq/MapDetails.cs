using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Device.Location;

namespace bkk_crawler_hq
{
    class MapDetails
    {
        private const int X = 10, Y = 6;

        private readonly Location LowerLeft = new Location() { Lat = 47.1523107, Lng = 18.8460594 };
        private readonly Location UpperRight = new Location() { Lat = 47.6837053, Lng = 19.391385500000002 };

        private double DiffLat => Math.Abs(UpperRight.Lat - LowerLeft.Lat) / X;
        private double DiffLng => Math.Abs(UpperRight.Lng - LowerLeft.Lng) / Y;

        public ConcurrentBag<Chunk> Chunks { get; private set; }

        public MapDetails()
        {
            Chunks = CalculateChunks();
        }

        public Weather getWeatherByTrip(Trip trip)
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
                    Location loc = new Location() {  Lat = lat, Lng = lng};
                    local.Add(new Chunk() { Center = loc });
                }
            }
            return local;
        }

        private Chunk MinimalDistance(Trip trip)
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
    }
}
