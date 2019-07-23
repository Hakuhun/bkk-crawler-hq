using bkk_crawler_hq;
using bkk_crawler_hq.Model;
using bkk_crawler_hq.Model.BKK;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace bkk_crawler_hq.Test
{
    [TestFixture]
    class MapDetailsTest 
    {
        MapDetails mapDetailsMock;

        [OneTimeSetUp]
        public void Init()
        {
            mapDetailsMock = MapDetails.GetInstance();
        }

        [TestCase(47.63056584, 19.209610133333335)]
        [TestCase(47.57742638, 19.11872245)]
        [TestCase(47.47546, 19.098385)]
        public void MinimalDistance_Test(double lat, double ln)
        {
            Trip trip = new Trip();
            VeichleData v = new VeichleData();
            Location l = new Location(lat, ln);
            v.Location = l;
            trip.Veichle = v;

            var asd = mapDetailsMock.MinimalDistance(trip);

            Assert.NotNull(asd);
        }

    }
}
