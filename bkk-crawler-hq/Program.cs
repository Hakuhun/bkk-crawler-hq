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
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
            
            Crawler crawler = new Crawler();
            sw.Start();
            crawler.getDataParallel();
            sw.Stop();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("A párhuzamosított modell: " + sw.Elapsed);
            crawler.clearData();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Egy perc várakozás az Időjárás API miatt");
            Thread.Sleep(60000);
            sw.Restart();
            crawler.GetDataSequential();
            sw.Stop();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("A szekvenciális modell: " + sw.Elapsed);
            sw.Restart();
            crawler.SerializeData();
            sw.Stop();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("A szerializáció: " + sw.Elapsed);
            Console.ReadLine();
        }

    }
}
