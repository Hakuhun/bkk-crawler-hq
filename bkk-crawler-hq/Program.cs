using bkk_crawler_hq.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using bkk_crawler_hq.Model.BKK;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace bkk_crawler_hq
{
    class Program
    {
        static int cycle = 0;
        public static long size = 0;
        static Crawler crawler;

        static System.Timers.Timer weatherTimer = new System.Timers.Timer();
        static System.Timers.Timer tripTimer = new System.Timers.Timer();
        static System.Timers.Timer routeTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            crawler = new Crawler();
            crawler.DownloadWeatherDatas();
            crawler.InitializeBaseRouteDatas();

            routeTimer.Elapsed += (sender, eventArgs) =>  crawler.InitializeBaseRouteDatas(); 
            routeTimer.Interval = 30000*10;
            routeTimer.Start();

            weatherTimer.Elapsed += (sender, eventArgs) => crawler.DownloadWeatherDatas();
            weatherTimer.Interval = 7500000;
            weatherTimer.Start();

            tripTimer.Elapsed += TripTimer_Elapsed;
            tripTimer.Interval = 30000;
            tripTimer.Start();
            

            

            string closingStatement = "";

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Az alkamazás fut, ("+cycle+") bezárásához adja meg az (X) karaktert");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                closingStatement = Console.ReadLine();
            } while (closingStatement != null && closingStatement.ToLower().Trim() != "x");

        }

        private static void TripTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.Clear();

            crawler.DownloadRouteDatas();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(crawler.Message);

            crawler.SerializeData();
            crawler.ClearData();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            cycle++;
            Console.WriteLine("Az alkamazás fut, (" + cycle + ") bezárásához adja meg az (X) karaktert");
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        public static double RouteTimerIntervall
        {
            get => routeTimer.Interval;
            set => tripTimer.Interval = value;
        }
    }
}
