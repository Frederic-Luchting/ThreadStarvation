using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Threading;

public class Program
{
    static int Requests;

    public static void Main(string[] args)
    {
        // Set thread pool limits BEFORE starting web host
        // Must set min threads FIRST, then max threads
        ThreadPool.SetMinThreads(10, 100);  
        ThreadPool.SetMaxThreads(10, 100);

        // Background thread to show thread pool stats every second
        new Thread(ShowThreadStats)
        {
            IsBackground = true
        }.Start();

        // Build and run the web API host
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.SetMinimumLevel(LogLevel.Critical);
        builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false);
        builder.WebHost.UseUrls("http://*:5000");
        var app = builder.Build();

        // Middleware to track active requests
        app.Use(async (context, next) =>
        {
            Interlocked.Increment(ref Requests);
            await next();
            Interlocked.Decrement(ref Requests);
        });

        app.UseMvc();
        app.Run();
    }

    // Background thread method to display thread pool stats
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
