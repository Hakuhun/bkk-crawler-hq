using bkk_crawler_hq.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using bkk_crawler_hq.Model.BKK;
using System.Diagnostics;
using System.Threading;

namespace bkk_crawler_hq
{
    class Program
    {
        static Crawler crawler;

        static void Main(string[] args)
        {
            System.Timers.Timer weatherTimer = new System.Timers.Timer();
            System.Timers.Timer routeTimer = new System.Timers.Timer();

            crawler = new Crawler();

            weatherTimer.Elapsed += WeatherTimer_Elapsed;
            weatherTimer.Interval = 7500000;
            weatherTimer.Start();

            routeTimer.Elapsed += RouteTimer_Elapsed;
            routeTimer.Interval = 300000;
            routeTimer.Start();
            Console.ReadLine();
        }

        private static void RouteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();

            crawler.DownloadRouteDatas();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(crawler.Message);

            crawler.SerializeData();
            crawler.clearData();
        }

        private static void WeatherTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            crawler.DownloadWeatherDatas();
        }
    }
}
