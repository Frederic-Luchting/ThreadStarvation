using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Threading.Controllers
{
    public class HomeController : Controller
    {
        private const int SleepOrDoNothingMilliseconds = 2000; // DB query, REST call,

        [HttpGet("/full-sync")] // Good old sync code 
        public string FullSync()
        {
            Thread.Sleep(SleepOrDoNothingMilliseconds); 
            return "Hello World";
        }

        [HttpGet("/async-over-sync")] // Modern async controller calling legacy sync services
        public async Task<string> AsyncOverSync()
        {
            await Task.Run(() => Thread.Sleep(SleepOrDoNothingMilliseconds));
            return "Hello World";
        }

        [HttpGet("/sync-over-async")] // Legacy sync code calling modern async APIs ☠️ DANGER!
        public string SyncOverAsync()
        {
            Task.Delay(SleepOrDoNothingMilliseconds).Wait();
            return "Hello World";
        }

        [HttpGet("/full-async")] // Full async/await throughout the stack
        public async Task<string> FullAsync()
        {
            await Task.Delay(SleepOrDoNothingMilliseconds);
            return "Hello World";
        }
    }
}
