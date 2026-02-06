# ThreadStarvation Demo Project - Thread Pool Configuration

## Project Context
- Repository: `c:\Users\luchtfr\source\repos\GitHub\nulllogicone\ThreadStarvation`
- Main demo project: `Threading` (ASP.NET Core web app on port 5000)
- Purpose: Demonstrates thread pool starvation issues
- Current target framework: net10.0

## Problem & Resolution

### Initial Issue
Threading app was running with 32,760 threads (default system maximum), which was too high for effective demo purposes.

### Solution Steps
1. **Thread pool limits must be set in `Main()` BEFORE web host starts**
   - Originally was in background thread `ShowThreadStats()` - too late
   - Moved to beginning of `Main(string[] args)`

2. **Order matters: SetMinThreads BEFORE SetMaxThreads**
   - `SetMaxThreads()` fails if value < current min threads
   - Correct order:
     ```csharp
     ThreadPool.SetMinThreads(10, 100);
     ThreadPool.SetMaxThreads(10, 100);
     ```

3. **Min threads control immediate availability**
   - Thread pool creates threads above min slowly (~1-2 per second)
   - With `SetMinThreads(1, 1)`: 8+ concurrent requests caused timeouts despite having max=10
   - With `SetMinThreads(10, 100)`: All threads immediately available, no timeouts
   
### Key Learning
**Thread Pool Behavior**: Min threads are pre-created and immediately available. Threads between min and max are created on-demand but slowly (injection rate ~1-2/sec). For burst traffic scenarios, low min threads cause timeouts even when max threads aren't exhausted.

### Current Configuration (Threading/Program.cs)
```csharp
ThreadPool.SetMinThreads(10, 100);  // Worker, IO threads - immediately available
ThreadPool.SetMaxThreads(10, 100);   // Maximum threads allowed
```

### Demo Tuning Options
- **Extreme starvation demo**: `SetMinThreads(1, 1); SetMaxThreads(2, 2);`
- **Show slow thread creation**: `SetMinThreads(3, 10); SetMaxThreads(10, 100);`
- **Reliable operation**: `SetMinThreads == SetMaxThreads`

### Additional Limiting Option
To limit CPU cores (not just threads), use processor affinity:
```powershell
Start-Process -FilePath "dotnet" -ArgumentList "run" -PassThru | ForEach-Object { $_.ProcessorAffinity = 1 }
```

## Project Structure Notes
- `Threading/Program.cs`: Main entry, thread pool stats monitoring
- `Threading/Startup.cs`: Request counting middleware
- `Requestor/`: Load testing tool for generating concurrent requests
- Demo tracks: Available, Active, Min, Max threads + Request count in console
