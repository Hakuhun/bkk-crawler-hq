using System;
using System.Collections.Generic;
using System.IO;
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
    public class SimpleTripData : ISimpleDataModel
    {
        private Trip trip;

        public SimpleTripData()
        {
                
        }

        public SimpleTripData(Trip trip)
        {
            this.trip = trip;
        }

        public long CurrentTime
        {
            get => trip.CurrentTime;
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

        public string getJSONFormat()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string getCSVFormat()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7}" + Environment.NewLine,
                CurrentTime, Latitude,Longitude, RouteID, TripID, VeichleID, Model, Status);
        }

        public string getCSVHeader()
        {
            return "CurrentTime;Latitude;Longitude;RouteID;TripID;VeichleID;Model;Status" + Environment.NewLine;
        }
    }
}
