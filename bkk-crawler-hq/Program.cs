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
        static int cycle = 0;
        public static long size = 0;
        static Crawler crawler;

        static System.Timers.Timer weatherTimer = new System.Timers.Timer();
        static System.Timers.Timer routeTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {

            crawler = new Crawler();

            weatherTimer.Elapsed += WeatherTimer_Elapsed;
            weatherTimer.Interval = 7500000;
            weatherTimer.Start();

            routeTimer.Elapsed += RouteTimer_Elapsed;
            routeTimer.Interval = 30000*2;
            routeTimer.Start();

            string closingStatement = "";

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Az alkamazás fut, ("+cycle+") bezárásához adja meg az (X) karaktert");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                closingStatement = Console.ReadLine();
            } while (closingStatement.ToLower().Trim() != "x");

        }

        private static void RouteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();

            crawler.DownloadRouteDatas();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(crawler.Message);

            crawler.SerializeData();
            crawler.clearData();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            cycle++;
            Console.WriteLine("Az alkamazás fut, (" + cycle + ") bezárásához adja meg az (X) karaktert");
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        private static void WeatherTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            crawler.DownloadWeatherDatas();
        }

        static public double RouteTimerIntervall
        {
            get
            {
                return routeTimer.Interval;
            }
            set
            {
                routeTimer.Interval = value;
            }
        }
    }
}
