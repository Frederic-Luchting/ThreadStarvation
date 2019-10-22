using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Requestor
{
    class Program
    {
        static HttpClient http = new HttpClient();
        //static int RequestCount = 0;
        static string path = "hello";
        static List<Thread> requestThreads = new List<Thread>();

        static async Task Main(string[] args)
        {
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0])) path = args[0];
            Console.WriteLine($"Hammering endpoint {path}");
            Console.WriteLine("Press ESC to stop, up- down- keys to change request count");
            http.Timeout = TimeSpan.FromMilliseconds(2500);

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                // do something
                var k = Console.ReadKey();
                if (k.Key == ConsoleKey.UpArrow)
                {
                    //RequestCount++;

                    var t = new Thread(RequestAndLogLoopAsync);
                    t.IsBackground = true;
                    t.Start();
                    requestThreads.Add(t);
                    Console.WriteLine();
                    Console.WriteLine(requestThreads.Count);
                }
                if (k.Key == ConsoleKey.DownArrow)
                {
                    if (requestThreads.Count > 0)
                    {
                        //RequestCount--;
                        var t = requestThreads[0];
                        t.Interrupt();
                        requestThreads.Remove(t);
                    }
                    Console.WriteLine();
                    Console.WriteLine(requestThreads.Count);
                }
            }
        }

        //static async Task  ParallelBatchRequestLoop()
        //{
        //    do{
        //        if (RequestCount == 0) continue;
        //        var sw = Stopwatch.StartNew();
        //        var requestTasks = new List<Task>();
        //        for (int i = 0; i < RequestCount; i++)
        //        {
        //            requestTasks.Add(RequestAndLogLoopAsync());
        //        }
        //        await Task.WhenAny(requestTasks);
        //        Console.WriteLine($"{RequestCount} requests in {sw.ElapsedMilliseconds}ms");
        //    }while(true);
        //}

        static void RequestAndLogLoopAsync()
        {
            try
            {
                while (true)
                {
                    var sw = Stopwatch.StartNew();
                    var res = http.GetAsync($"http://localhost:5000/{path}").Result;
                    var cnt = res.Content.ReadAsStringAsync().Result;
                    Console.Write($"{sw.ElapsedMilliseconds}, ");
                }
            }
            catch (ThreadInterruptedException tie) 
            { }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
