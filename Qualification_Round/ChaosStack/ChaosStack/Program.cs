using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace ChaosStack
{
    class Program
    {
        private const string api_key = "6ZC9jyeffYUudGnrUNSRoijAL7R2";
        private const string base_url = " https://us-central1-chaosstack.cloudfunctions.net/";
        private const string destination = "47.478373, 19.048594";

        static JObject CalculateSolution(JObject jsonGenResponse)
        {
            var solution = new JObject();
            solution["solutions"] = new JArray();
            var solutions = solution["solutions"] as JArray;

            foreach (JArray tests in jsonGenResponse["tests"])
            {
                int furthestIdx = 0;
                int furthestDur = 0;
                for (int i = 0; i < tests.Count; i++)
                {
                    var test = tests[i];
                    Thread.Sleep(1);
                    var gMapsRes = ManageGMaps.GetGMapsResult(test["lat"].ToString(), test["lon"].ToString(), destination);
                    var dur = int.Parse(((gMapsRes["rows"] as JArray)[0]["elements"] as JArray)[0]["duration"]["value"].ToString()) / 60;
                    if (dur > furthestDur)
                    {
                        furthestIdx = i;
                        furthestDur = dur;
                    }
                }
                var result = new JObject();
                result["farthestIndex"] = furthestIdx;
                result["farthestDuration"] = furthestDur;
                solutions.Add(result);
            }

            return solution;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var apiHeader = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Authorization", api_key) };
            var genResponse = HTTP_Requests.Send_GetRequest(base_url + "/generate", apiHeader);
            var jsonGenResponse = JObject.Parse(genResponse);
            var testSuiteToken = jsonGenResponse["testSuiteToken"];

            var solution = CalculateSolution(jsonGenResponse);

            var result = HTTP_Requests.Send_PostRequest(base_url + "/submit?token=" + testSuiteToken, solution.ToString(), "application/json", apiHeader);
            Console.WriteLine(result);
            Clipboard.SetText(result);
            Console.ReadKey();
        }
    }
}
