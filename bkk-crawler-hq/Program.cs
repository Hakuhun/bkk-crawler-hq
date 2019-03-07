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
            Console.ReadLine();

            int i = 0;
            while (true) {
                crawler = new Crawler();
                Console.Clear();
                Console.WriteLine(string.Format("{0}. ciklus",i));
                crawler.getDataParallel();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(crawler.Message);

                crawler.SerializeData();
                crawler.clearData();

                Console.WriteLine("Egy perc várakozás az Időjárás API miatt");
                Thread.Sleep(60001);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                i++;
            }

            Console.ReadLine();
        }

    }
}
