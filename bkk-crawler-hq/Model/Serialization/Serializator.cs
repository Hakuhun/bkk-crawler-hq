using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using bkk_crawler_hq.Model.BKK;
using bkk_crawler_hq.Model.Parquet;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using Parquet;
using Parquet.Data;
using DataColumn = Parquet.Data.DataColumn;
/// <summary>
/// APEEND example: https://github.com/elastacloud/parquet-dotnet/blob/master/doc/writing.md
/// </summary>


namespace bkk_crawler_hq.Model
{
    class Serializator
    {
        public static void SerializeCollectionToCSV(IEnumerable<ISimpleDataModel> list, string path)
        {
            string text = "";
            if (!File.Exists(path))
                text = list.FirstOrDefault().getCSVHeader();

            foreach (ISimpleDataModel data in list)
            {
                text += data.getCSVFormat();
            }

            File.AppendAllText(path, text, Encoding.UTF8);
        }

        public static void SerializeCollectionToJSON(IEnumerable<ISimpleDataModel> list, string path)
        {
            List<dynamic> existing = null;
            try
            {
                existing = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(path));
            }
            catch (FileNotFoundException)
            {
                existing = new List<dynamic>(list);
                File.WriteAllText(path, JsonConvert.SerializeObject(existing));
            }

            if (existing != null)
            {
                existing.AddRange(list);
                File.WriteAllText(path, JsonConvert.SerializeObject(existing));
            }
        }

        //public static void SaveTripsToParquetShema(IEnumerable<SimpleTripData> data)
        //{
        //    var ct = new DataField<long>("CurrentTime");
        //    var lat = new DataField<double>("Latitude");
        //    var lng = new DataField<double>("Longitude");
        //    var route = new DataField<string>("RouteID");
        //    var trip = new DataField<string>("TripID");
        //    var veichle = new DataField<string>("VeichleID");
        //    var model = new DataField<string>("Model");
        //    var status = new DataField<string>("Status");

        //    var ms = new MemoryStream();
        //    ms.Position = 0;

        //    using (var writer = new ParquetWriter(new Schema(ct, lat, lng, route, trip, veichle, model, status), ms,new ParquetOptions(), append:true))
        //    {
        //        using (ParquetRowGroupWriter rg = writer.CreateRowGroup())
        //        {
        //            rg.WriteColumn(new DataColumn(ct, data.Select((x) => x.CurrentTime).ToArray<long>()));
        //            rg.WriteColumn(new DataColumn(lat, data.Select((x) => x.Latitude).ToArray<double>()));
        //            rg.WriteColumn(new DataColumn(lng, data.Select((x) => x.Longitude).ToArray<double>()));
        //            rg.WriteColumn(new DataColumn(route, data.Select((x) => x.RouteID).ToArray<string>()));
        //            rg.WriteColumn(new DataColumn(trip, data.Select((x) => x.TripID).ToArray<string>()));
        //            rg.WriteColumn(new DataColumn(veichle, data.Select((x) => x.VeichleID).ToArray<string>()));
        //            rg.WriteColumn(new DataColumn(model, data.Select((x) => x.Model).ToArray<string>()));
        //            rg.WriteColumn(new DataColumn(status, data.Select((x) => x.Status).ToArray<string>()));
        //        }
        //    }

        //    ms.CopyTo(File.OpenWrite(trip_path));
        //}
    }
}
