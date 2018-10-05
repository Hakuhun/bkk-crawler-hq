using bkk_crawler_hq.Model;
using System;
using System.Threading.Tasks;

namespace bkk_crawler_hq
{
    class Program
    {
        static void Main(string[] args)
        {
            WCrawler w = new WCrawler();
            Weather asd;
            new Task(async () =>
            {
                asd = w.getWeatherByGeoTags(47.46, 19.01).Result;
                Console.WriteLine(asd.WeatherCondition);
            }).Start();
            Console.ReadLine();
        }
    }
}
