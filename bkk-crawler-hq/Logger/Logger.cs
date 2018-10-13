using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace bkk_crawler_hq.Logger
{
    class Logger
    {
        
        public Logger()
        {
            
        }

        public Exception Exception { get; set; }

        public string RouteId { get; set; }

        public string Url { get; set; }

        public string Message => string.Format("Hiba történt: {0}\n{1} {2} időben @ {3} vonalon.\nAPI: {4}\n", Exception.ToString(), DateTime.Now, RouteId, Exception.Message, Url);

        public void Write()
        {
            Debug.Print(this.Message);
            Console.WriteLine(this.Message);
            WriteLine(this.Message);
        }

        private void WriteLine(string text)
        {
            StreamWriter writer = new StreamWriter("log.bkk",true);
            writer.WriteAsync(text);
            writer.Close();
        }
    }
}
