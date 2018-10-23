using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using bkk_crawler_hq.Model.Parquet;

namespace bkk_crawler_hq.Model
{
    class Weather
    {
        private MainWeatherData main;

        /// <summary>
        /// Latitude of the measured weather 
        /// Description of the datatypes: https://openweathermap.org/weather-data   
        /// </summary>
        private double? latitude;

        /// <summary>
        /// Longitude of the measured weather 
        /// </summary>
        private double? _longitude;

        /// <summary>
        /// Current timestamp when the weather was measured 
        /// </summary>
        private long? currentTime;

        private WindData wind;

        private RainData rain;

        private SnowData snow;

        public Weather()
        {
                
        }

        public Weather(double? lat = null, double? lng = null, long? dt = null)
        {
            this.latitude = lat;
            this._longitude = lng;
            this.currentTime = dt;
        }

        [JsonProperty("main")]
        public MainWeatherData Main { get => main; set => main = value; }

        [JsonProperty("wind")]
        public WindData Wind { get => wind; set => wind = value; }

        [JsonProperty("clouds")]
        public CloudData Cloud { get; set; }

        [JsonProperty("rain")]
        public RainData Rain { get => rain; set => rain = value; }

        [JsonProperty("snow")]
        public SnowData Snow { get => snow; set => snow = value; }

        [JsonProperty("weather")]
        private JArray Raw { get; set; }

        /// <summary>
        /// Overall condition of the weather in the area
        /// See more at https://openweathermap.org/weather-conditions
        /// </summary>
        public string WeatherCondition { get => Raw.First.Value<string>("main"); }

        public double? Latitude
        {
            get => latitude;
            set {
                latitude = value;
            }
        }

        public double? Longitude
        {
            get => _longitude;
            set
            {
                this._longitude = value;
            }
        }

        [JsonProperty("dt")]
        public long? CurrentTime
        {
            get => currentTime;
            set
            {
                this.currentTime = value;
            }
        }

        public SimpleWeatherData getParquetFormat()
        {
            return new SimpleWeatherData(this);
        }

        internal class SnowData
        {

            /// <summary>
            /// Snow volume of the last 3 hour
            /// </summary>
            private int? snow;

            [JsonProperty("3h")]
            public int? Snow { get => snow; set => snow = value; }
        }

        internal class RainData
        {
            /// <summary>
            /// Rain volume of the last 3 hour
            /// </summary>
            private int? rain;

            [JsonProperty("3h")]
            public int? Rain { get => rain; set => rain = value; }
        }

        internal class CloudData
        {
            //TODO: IS IT IMPORTANT? 
            /// <summary>
            /// Cloudiness in percent
            /// 
            /// %
            /// </summary>
            private int cloud;

            [JsonProperty("all")]
            public int Cloud { get => cloud; set => cloud = value; }
        }

        internal class MainWeatherData
        {
            /// <summary>
            /// Current weather temperature in Celsius
            /// Celsius
            /// </summary>
            private double temperature;

            /// <summary>
            /// Humidity of te current measured weather
            /// %
            /// </summary>
            private int humidity;

            /// <summary>
            /// Preasure of the location
            /// hPa
            /// </summary>
            private double pressure;

            [JsonProperty("temp")]
            public double Temperature { get => temperature; set => temperature = value; }

            [JsonProperty("humidity")]
            public int Humidity { get => humidity; set => humidity = value; }

            [JsonProperty("pressure")]
            public double Pressure { get => pressure; set => pressure = value; }

        }

        internal class WindData
        {
            //TODO: IS IT IMPORTANT? 
            /// <summary>
            /// Wind speed
            /// meter/sec
            /// </summary>
            private double windSpeed;

            //TODO: IS IT IMPORTANT? 
            /// <summary>
            /// Wind direction
            /// degrees (meteorological)
            /// </summary>
            private double windDegree;

            [JsonProperty("speed")]
            public double WindSpeed { get => windSpeed; set => windSpeed = value; }

            [JsonProperty("deg")]
            public double WindDegree { get => windDegree; set => windDegree = value; }
        }
    }
}
