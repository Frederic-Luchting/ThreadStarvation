using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Threading.Controllers
{
    public class HomeController : Controller
    {
        private const int SleepOrDoNothingMilliseconds = 2000;

        [HttpGet("/full-sync")] // Good old sync code like in admin and apis
        public string Hello()
        {
            Thread.Sleep(SleepOrDoNothingMilliseconds); // DB query, REST call,
            return "Hello World";
        }

        [HttpGet("/async-over-sync")] // New project using old services
        public async Task<string> HelloAsyncOverSync()
        {
            await Task.Run(() => Thread.Sleep(SleepOrDoNothingMilliseconds));
            return "Hello World";
        }

        [HttpGet("/sync-over-async")] // Old code using new APIs
        public string HelloSyncOverAsync()
        {
            Task.Delay(SleepOrDoNothingMilliseconds).Wait();
            return "Hello World";
        }

        [HttpGet("/full-async")] // All new
        public async Task<string> HelloAsync()
        {
            await Task.Delay(SleepOrDoNothingMilliseconds);
            return "Hello World";
        }
    }
}
