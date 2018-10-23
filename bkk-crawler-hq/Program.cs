using bkk_crawler_hq.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using bkk_crawler_hq.Model.BKK;

namespace bkk_crawler_hq
{
    class Program
    {

        static void Main(string[] args)
        {
            Crawler crawler = new Crawler();
            crawler.getData();
            //Console.ReadLine();
        }

    }
}
