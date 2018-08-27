using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ChaosStack
{
    class HTTP_Requests
    {
        public static string Send_GetRequest(string uri, List<KeyValuePair<string, string>> headers = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string Send_PostRequest(string uri, string data, string contentType, List<KeyValuePair<string, string>> headers = null)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = "POST";

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
