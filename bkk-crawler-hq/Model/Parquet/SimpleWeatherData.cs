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

        public int SnowingIntensity
        {
            get => weather.Snow.Snow;
        }

        public int RainingIntensity
        {
            get => weather.Rain.Rain;
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
