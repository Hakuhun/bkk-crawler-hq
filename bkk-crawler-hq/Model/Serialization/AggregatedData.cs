namespace bkk_crawler_hq.Model.Serialization
{
    public class AggregatedData : ISimpleDataModel
    {
        private SimpleCollectedData data;

        public AggregatedData(SimpleCollectedData data)
        {
            this.data = data;
        }

        public string GetCsvFormat()
        {
            throw new System.NotImplementedException();
        }

        public string GetCsvHeader()
        {
            throw new System.NotImplementedException();
        }
    }
}