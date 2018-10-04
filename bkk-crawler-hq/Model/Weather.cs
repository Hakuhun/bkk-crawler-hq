using System;
using System.Collections.Generic;
using System.Text;

namespace bkk_crawler_hq.Model
{
    class Weather
    {
        /// <summary>
        /// Latitude of the measured weather 
        /// Description of the datatypes: https://openweathermap.org/weather-data   
        /// </summary>
        private long latitude;

        /// <summary>
        /// Longitude of the measured weather 
        /// </summary>
        private long longutide;

        /// <summary>
        /// Current timestamp when the weather was measured 
        /// </summary>
        private long timestamp;

        /// <summary>
        /// Current weather temperature in Celsius
        /// Celsius
        /// </summary>
        private int temperature;

        /// <summary>
        /// Overall condition of the weather in the area
        /// See more at https://openweathermap.org/weather-conditions
        /// </summary>
        private string weatherCondition;

        /// <summary>
        /// Humidity of te current measured weather
        /// %
        /// </summary>
        private int humidity;

        /// <summary>
        /// Preasure of the location
        /// hPa
        /// </summary>
        private int pressure;

        //TODO: IS IT IMPORTANT? 
        /// <summary>
        /// Wind speed
        /// meter/sec
        /// </summary>
        private int windSpeed;

        //TODO: IS IT IMPORTANT? 
        /// <summary>
        /// Wind direction
        /// degrees (meteorological)
        /// </summary>
        private int windDegree;

        //TODO: IS IT IMPORTANT? 
        /// <summary>
        /// Cloudiness in percent
        /// 
        /// %
        /// </summary>
        private int cloud;

        /// <summary>
        /// Rain volume of the last 3 hour
        /// </summary>
        private int rain;

        /// <summary>
        /// Snow volume of the last 3 hour
        /// </summary>
        private int snow;

        public long Latitude { get => latitude; set => latitude = value; }
        public long Longutide { get => longutide; set => longutide = value; }
        public long Timestamp { get => timestamp; set => timestamp = value; }
        public int Temperature { get => temperature; set => temperature = value; }
        public string WeatherCondition { get => weatherCondition; set => weatherCondition = value; }
        public int Humidity { get => humidity; set => humidity = value; }
        public int Pressure { get => pressure; set => pressure = value; }
        public int WindSpeed { get => windSpeed; set => windSpeed = value; }
        public int WindDegree { get => windDegree; set => windDegree = value; }
        public int Cloud { get => cloud; set => cloud = value; }
        public int Rain { get => rain; set => rain = value; }
        public int Snow { get => snow; set => snow = value; }
    }
}
