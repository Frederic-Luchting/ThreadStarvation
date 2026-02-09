using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Requestor;

class Program
{
    private const int HttpClientTimeoutMilliseconds = 2700; 
    static string path = "full-sync";

    static readonly HttpClient http = new HttpClient(); // Another anti-pattern that could cause socket exhaustion. Here it's fine, but in production consider using IHttpClientFactory or similar pattern.
    static readonly List<Thread> requestThreads = [];

    static void Main(string[] args)
    {
        // set the path from command arguments, dotnet run <path>
        // possible values: full-sync (default), async-over-sync, sync-over-async, full-async
        if (args.Length > 0 && !string.IsNullOrEmpty(args[0])) path = args[0];
        Console.WriteLine();
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Hammering endpoint: /{path}");
        Console.WriteLine("Press ESC to stop, up- down- keys to change request count");
        http.Timeout = TimeSpan.FromMilliseconds(HttpClientTimeoutMilliseconds);

        while (true)
        {
            var k = Console.ReadKey();

            if (k.Key == ConsoleKey.Escape)
            {
                break;
            }
            if (k.Key == ConsoleKey.UpArrow)
            {
                AddRequestThread();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine($"Request {path}: " + requestThreads.Count(t => t.IsAlive == true));
                Console.ResetColor();
            }
            if (k.Key == ConsoleKey.DownArrow)
            {
                if (requestThreads.Count > 0)
                {
                    requestThreads.RemoveAll(dt => dt.IsAlive == false);
                    if (requestThreads.Count == 0) continue;
                    var t = requestThreads[0];
                    t.Interrupt();
                    requestThreads.Remove(t);
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine();
                Console.WriteLine($"Requests {path}: " + requestThreads.Where(t => t.IsAlive == true).Count());
                Console.ResetColor();
            }
        }
    }

    // Hammer an endpoint, one request after another, and log the time it took for the response.
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
        catch (ThreadInterruptedException)
        {
            requestThreads.RemoveAll(dt => dt.IsAlive == false);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message + ex.StackTrace);
            Console.ResetColor();
            // put thread back after crash
            AddRequestThread();
        }
    }

    // Add a new background thread to the pool that hammers the endpoint in parallel
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
