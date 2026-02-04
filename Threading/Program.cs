using System;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Threading
{
    public class Program
    {
        public static int Requests;

        public static void Main(string[] args)
        {
            // Set thread pool limits BEFORE starting web host
            // Must set min threads FIRST, then max threads
            ThreadPool.SetMinThreads(10, 100);  // Match max for immediate availability
            var success = ThreadPool.SetMaxThreads(10, 100);
            Console.WriteLine($"SetMaxThreads(10, 100) returned: {success}");
            
            ThreadPool.GetMaxThreads(out var maxWorker, out var maxIO);
            Console.WriteLine($"Actual Max Threads: Worker={maxWorker}, IO={maxIO}");

            new Thread(ShowThreadStats)
            {
                IsBackground = true
            }.Start();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://*:5000")
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Critical);
                })
                .Build();

        private static void ShowThreadStats(object obj)
        {
            while (true)
            {
                ThreadPool.GetAvailableThreads(out var workerThreads, out var _);
                ThreadPool.GetMinThreads(out var minThreads, out var _);
                ThreadPool.GetMaxThreads(out var maxThreads, out var _);

                Console.WriteLine($"Avail: {workerThreads}, Active: {maxThreads - workerThreads}, Min: {minThreads}, Max: {maxThreads}, Req: {Requests}");

                Thread.Sleep(1000);
            }
        }
    }
}
