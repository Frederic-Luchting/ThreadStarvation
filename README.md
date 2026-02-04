# Thread starvation - async/await

This repository comes from a good presentation of Damian Edwards and David Fowler (two awesome guys from the .Net Core team) about Diagnosing issues in your code.

It's a little bit outdated (2018) but aged very well. For latest updates see bottom 

Simple story, there are two projects:

- **Threading** is a little Hello world web application with 4 methods
  - Good old full synchronous execution, works but not very efficient
  - Modern app with legacy services: async-over-sync, not best but works
  - Old app code using modern apis: sync-over-async, shows dead IIS (☠) because of thread starvation
  - Full async, shows unlimited web scale
- **Requestor** is hammering an endpoint. You can arrow up/down to scale

[Source code]  
https://github.com/davidfowl/NdcLondon2018

[Youtube]
https://www.youtube.com/watch?v=RYI0DHoIVaA

## Howto use it

- Open the repository folder in VS Code
- Open terminal (CTRL+`), split in two
- Navigate in the left to ~/Requestor (left)
- Navigate in the right to ~/Threading (right)

Then you can start the Threading monitor on the right with `dotnet run` and it shows Available threads and handled requests.
On the left side you can `dotnet run <method name>` where method name is one of the following and arrow up/down

|dotnet run \<parameter> | comment|
|---------|---|
|hello                    | good old sync |
|hello-async-over-sync    | modern controller over old services |
|hello-sync-over-async    | ☠ can kill the server, must close terminal |
|hello-async              | scales unlimited!|

![VSC](https://raw.githubusercontent.com/nulllogicone/ThreadStarvation/master/images/VS_Code.PNG)

## Latest updates (2026)

- Bump to dotnet 10.0
- Fix MaxThread setting
- Add copilot chat summary to memory markdown (!!!)
