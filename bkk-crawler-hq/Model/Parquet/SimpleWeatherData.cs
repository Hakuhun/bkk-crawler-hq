using System;
using System.Collections.Generic;
using System.Text;

namespace bkk_crawler_hq.Model.Parquet
{
    class SimpleWeatherData
    {
        public SimpleWeatherData()
        {
            
        }

        private Weather weather;

        public SimpleWeatherData(Weather weather)
        {
            this.weather = weather;
        }

        public string RouteID { get; set; }

        public string TripID { get; set; }

        public double? Latitude
        {
            get => weather.Latitude;
        }

        public double? Longutide
        {
            get => weather.Longitude;
        }

        public long? CurrentTime
        {
            get => weather.CurrentTime;
        }

        public int? SnowingIntensity
        {
            get
            {
                try
                {
                    return weather.Snow.Snow;
                }
                catch (NullReferenceException)
                {
                    return 0;
                }
            }
        }

        public int? RainingIntensity
        {
            get
            {
                try
                { 
                    return weather.Rain.Rain;
                }
                catch (NullReferenceException)
                {
                    return 0;
                }

            }
        }

        public double Temperature
        {
            get => weather.Main.Temperature;
        }

        public int Humidity
        {
            get => weather.Main.Humidity;

        }

        public double Pressure
        {
            get => weather.Main.Pressure;
        }

        public double WindSpeed
        {
            get => weather.Wind.WindSpeed;
        }

        public double WindDegree
        {
            get => weather.Wind.WindDegree;
        }
    }
}
