using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requestor
{
    class Program
    {
        static HttpClient http = new HttpClient();
        static int RequestCount = 0;
        static string path = "hello";

        static async Task Main(string[] args)
        {
            if(args.Length> 0 && !string.IsNullOrEmpty(args[0])) path = args[0];
            Console.WriteLine($"Hammering endpoint {path}");
            Console.WriteLine("Press ESC to stop, up- down- keys to change request count");
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                await ParallelBatchRequestLoop();
            }).Start();
     
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                // do something
                var k = Console.ReadKey();
                if (k.Key == ConsoleKey.UpArrow)
                {
                    RequestCount++;
                    Console.WriteLine(RequestCount);
                }
                if (k.Key == ConsoleKey.DownArrow)
                {
                    if (RequestCount > 0) RequestCount--;
                    Console.WriteLine(RequestCount);
                }
            }
        }

        static async Task  ParallelBatchRequestLoop()
        {
            do{
                if (RequestCount == 0) continue;
                var sw = Stopwatch.StartNew();
                var requestTasks = new List<Task>();
                for (int i = 0; i < RequestCount; i++)
                {
                    requestTasks.Add(RequestAndLogAsync());
                }
                await Task.WhenAll(requestTasks);
                Console.WriteLine($"{RequestCount} requests in {sw.ElapsedMilliseconds}ms");
                await Task.Delay(10);
            }while(true);
        }

        static async Task RequestAndLogAsync()
        {
            var sw = Stopwatch.StartNew();
            var res = await http.GetAsync($"http://localhost:5000/{path}");
            var cnt = await res.Content.ReadAsStringAsync();
            Console.Write($"{sw.ElapsedMilliseconds} ms, ");
        }
    }
}
