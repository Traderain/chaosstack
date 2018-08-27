using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChaosStack
{
    class Program
    {
        private const string api_key = "6ZC9jyeffYUudGnrUNSRoijAL7R2";
        private const string base_url = " https://us-central1-chaosstack.cloudfunctions.net/";

        private const string url_part = "/generate";
        
        [STAThread]
        static void Main(string[] args)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(base_url+url_part);
            request.Headers.Add("Authorization",api_key);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            string json;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }
            Console.WriteLine(json);
            Clipboard.SetText(json);
            Console.ReadLine();
        }
    }
}
