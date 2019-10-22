using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Requestor
{
    class Program
    {
        static HttpClient http = new HttpClient();
        //static int RequestCount = 0;
        static string path = "hello";
        static List<Thread> requestThreads = new List<Thread>();

        static void Main(string[] args)
        {
            if (args.Length > 0 && !string.IsNullOrEmpty(args[0])) path = args[0];
            Console.WriteLine($"Hammering endpoint {path}");
            Console.WriteLine("Press ESC to stop, up- down- keys to change request count");
            http.Timeout = TimeSpan.FromMilliseconds(2500);

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                var k = Console.ReadKey();
                if (k.Key == ConsoleKey.UpArrow)
                {
                    AddRequestThread();
                    Console.WriteLine();
                    Console.WriteLine("Requests: " + requestThreads.Where(t => t.IsAlive == true).Count());
                }
                if (k.Key == ConsoleKey.DownArrow)
                {
                    if (requestThreads.Count > 0)
                    {
                        //RequestCount--;
                        requestThreads.RemoveAll(dt => dt.IsAlive == false);
                        if (requestThreads.Count == 0) continue;
                        var t = requestThreads[0];
                        t.Interrupt();
                        requestThreads.Remove(t);
                    }
                    Console.WriteLine();
                    Console.WriteLine("Requests: " + requestThreads.Where(t => t.IsAlive == true).Count());
                }
            }
        }

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
            {
                requestThreads.RemoveAll(dt => dt.IsAlive == false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                // put thread back after crash
                AddRequestThread();
            }
        }

        private static void AddRequestThread()
        {
            var t = new Thread(RequestAndLogLoopAsync)
            {
                IsBackground = true
            };
            t.Start();
            requestThreads.Add(t);
        }
    }
}
