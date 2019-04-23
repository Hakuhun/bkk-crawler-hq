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
            text.Replace(',','.');
            File.AppendAllText(path, text, Encoding.UTF8);
            text = null;
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
    }
}
