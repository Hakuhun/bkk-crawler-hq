using bkk_crawler_hq.Model;
using System;
using System.Threading.Tasks;
using bkk_crawler_hq.Model.BKK;

namespace bkk_crawler_hq
{
    class Program
    {
        static void Main(string[] args)
        {
            WCrawler w = new WCrawler();
            BKKCrawler bkk = new BKKCrawler();
            Weather asd;
            Trip trytrip;
            new Task(async () =>
            {
                try
                {
                    Task<Trip> asd2 = bkk.getDetailedTripData("BKK_B8428358x", "BKK_6229");
                    trytrip = await asd2;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                //asd = await w.getWeatherByGeoTags(47.46, 19.01);
                //Console.WriteLine(asd.WeatherCondition);
            }).Start();
            Console.ReadLine();
        }

    }
}
