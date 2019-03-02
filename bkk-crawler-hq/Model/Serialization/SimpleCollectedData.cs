using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Model.Parquet;
using System;
using System.Collections.Generic;
using System.Text;

namespace bkk_crawler_hq.Model.Serialization
{
    class SimpleCollectedData : ISimpleDataModel
    {
        public Trip Trip { get; set; }
        public Weather Weather { get; set; }

        public SimpleTripData STrip { get; set; }
        public SimpleWeatherData SWeather { get; set; }
        public List<StopData> Stops { get; set; }

        public string getCSVFormat()
        {
            StringBuilder sb = new StringBuilder();

            string baseTripData = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                STrip.CurrentTime, STrip.RouteID, STrip.TripID, STrip.VeichleID,
                STrip.VeichleType, STrip.Model, STrip.Status,
                STrip.Longitude, STrip.Latitude
            );
            string baseWeatherData = string.Format("{0};{1};{2};{3};{4};{5};",
                    SWeather.Temperature, SWeather.Humidity, SWeather.Pressure,
                    SWeather.WindSpeed, SWeather.SnowingIntensity, SWeather.SnowingIntensity
                );
            foreach (StopData stop in Stops)
            {
                string localStopData = string.Format("{0};{1};{3};{4};{5};",
                        stop.StopId, stop.StopSquence, stop.PredictedArrivalTime, 
                        stop.ArrivalTime, stop.PredictedDepartureTime, stop.DepartureTime
                    );
                sb.Append(baseTripData);
                sb.Append(localStopData);
                sb.Append(baseWeatherData);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public string getCSVHeader()
        {
            return string.Format(
                "CurrentTime;" +
                "RouteID;" +
                "TripID;" +
                "VeichleID;" +
                "VeichleType;" +
                "VeichleModel;" +
                "TripStatus;" +
                "VeichleLongitude" +
                "VeichleLatitude" +
                "StopID;" +
                "StopSequance;" +
                "PredictedArrivalTime;" +
                "ArrivalTime;" +
                "PredictedDepartureTime;" +
                "DepartureTime;" +
                "Temperature;" +
                "Humidity;" +
                "Preasure;" +
                "WindIntensity;" +
                "SnowIntesity;" +
                "RainIntensity"
                );
        }
    }
}
