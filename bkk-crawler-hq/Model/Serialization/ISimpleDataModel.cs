using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace bkk_crawler_hq.Model
{
    public interface ISimpleDataModel
    {
        string GetCsvFormat();
        string GetCsvHeader();

        //void SerializeCollectionToJSON(IEnumerable<ISimpleDataModel> list, string path);
        //void SerializeCollectionToCSV(IEnumerable<ISimpleDataModel> list, string path, bool header = false);
    }
}
