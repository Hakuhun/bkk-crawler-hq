using bkk_crawler_hq.Model;
using System;

namespace bkk_crawler_hq
{
    class Program
    {
        static void Main(string[] args)
        {
            WCrawler w = new WCrawler();
            Weather asd = w.getWeatherByGeoTags(19.01, 47.46).Result;
            Console.ReadLine();
        }
    }
}
