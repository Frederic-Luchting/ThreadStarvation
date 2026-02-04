# Thread starvation - async/await

This repository comes from a good presentation of Damian Edwards and David Fowler (two awesome guys from the .Net Core team) about Diagnosing issues in your code.

It's a little bit outdated (2018) but aged very well. For latest updates see bottom 

## Project Structure

```
ThreadStarvation/
├── Threading/          # Backend web server with test endpoints
├── Requestor/          # Load testing client tool
├── Talk1Samples/       # Original NDC London 2018 samples
└── Logging/            # Logging utilities
```

## The Two Projects

- **[Threading](Threading/readme.md)** - A Hello World web application with 4 methods demonstrating different async patterns:
  - Good old full synchronous execution, works but not very efficient
  - Modern app with legacy services: async-over-sync, not best but works
  - Old app code using modern APIs: sync-over-async, shows dead IIS (☠) because of thread starvation
  - Full async, shows unlimited web scale
- **[Requestor](Requestor/readme.md)** - Load testing tool that hammers the Threading endpoints. You can arrow up/down to scale parallel requests

## Original Source

[Source code]  
https://github.com/davidfowl/NdcLondon2018

[Youtube]
https://www.youtube.com/watch?v=RYI0DHoIVaA

## How to Use It

### Setup
1. Open the repository folder in VS Code
2. Open terminal (CTRL+`), split in two
3. Navigate to `~/Requestor` in the left terminal
4. Navigate to `~/Threading` in the right terminal

### Running the Demo
1. **Start the server** (right terminal): `dotnet run`
   - Displays available threads and handled requests in real-time
2. **Run load test** (left terminal): `dotnet run <method name>`
   - Use arrow ⬆/⬇ keys to increase/decrease parallel request load

### Available Test Endpoints

|Command | Pattern | Result|
|--------|---------|-------|
|`dotnet run full-sync` | Traditional synchronous execution | ✅ Works but limited scalability |
|`dotnet run async-over-sync` | Modern async controller calling legacy sync services | ⚠️ Suboptimal but functional |
|`dotnet run sync-over-async` | Legacy sync code calling modern async APIs | ☠️ **DANGER**: Thread pool starvation! |
|`dotnet run full-async` | Full async/await throughout the stack | 🚀 Unlimited scalability! |

> **⚠️ Warning**: The `sync-over-async` endpoint demonstrates thread pool starvation and can deadlock the server. You may need to forcefully close the terminal to recover.

![VSC](https://raw.githubusercontent.com/nulllogicone/ThreadStarvation/master/images/VS_Code.PNG)

## Key Learnings

- **Never block on async code** - Using `.Wait()` or `.Result` on Tasks leads to thread pool starvation
- **Async all the way** - If you have async APIs, use them asynchronously throughout the entire call stack
- **Thread pool threads are precious** - Don't waste them waiting synchronously for I/O operations
- **Async-over-sync is acceptable** - When you must call legacy synchronous code from modern async APIs
- **Monitor your thread pool** - Use `ThreadPool.GetAvailableThreads()` to detect starvation issues early

This demo clearly shows the performance impact of different async patterns and why proper async/await usage is critical for scalable web applications.

## Latest updates (2026)

- Bump to dotnet 10.0
- Fix MaxThread setting
- Add copilot chat summary to memory markdown (!!!)
