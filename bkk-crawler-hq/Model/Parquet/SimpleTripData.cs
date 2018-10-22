using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using bkk_crawler_hq.Model.BKK;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Parquet;
using Parquet.Data;

/// <summary>
/// https://www.confluent.io/blog/decoupling-systems-with-apache-kafka-schema-registry-and-avro/
/// </summary>

namespace bkk_crawler_hq.Model
{
    public class SimpleTripData
    {
        private Trip trip;

        public SimpleTripData()
        {
                
        }

        public long CurrentTime
        {
            get => trip.CurrentTime;
        }
        
        public SimpleTripData(Trip trip)
        {
            this.trip = trip;
        }

        public double Latitude
        {
            get => trip.Veichle.Location.Lat;
        }

        public double Longitude
        {
            get => trip.Veichle.Location.Lng;
        }

        public string RouteID
        {
            get => trip.RouteID;
        }

        public string TripID
        {
            get => trip.Veichle.TripID;
        }

        public string VeichleID
        {
            get => trip.Veichle.TripID;
        }

        public string Model
        {
            get => trip.Veichle.Model;
        }

        public string Status
        {
            get => trip.Veichle.Status;
        }


    }
}
