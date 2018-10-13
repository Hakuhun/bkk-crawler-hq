using bkk_crawler_hq.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Logger;

namespace bkk_crawler_hq
{
    class Program
    {
        public static Logger.Logger logger = new Logger.Logger();

        static void Main(string[] args)
        {
            WCrawler w = new WCrawler();
            BKKCrawler bkk = new BKKCrawler();
            Weather asd;
            List<Trip> tripdata;
            new Task(async () =>
            {
            //    try
            //    {
            //        Task<Trip> asd2 = bkk.getDetailedTripData("BKK_B8428358x", "BKK_6229");
            //        trytrip = await asd2;
            //        Console.WriteLine(trytrip.Veichle.Status);
            //    }
            //    catch (BKKExcepton e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //    asd = await w.getWeatherByGeoTags(47.46, 19.01);
            //    Console.WriteLine(asd.WeatherCondition);
                tripdata = bkk.getTrips();
            }).Start();
            Console.ReadLine();
        }

    }
}
