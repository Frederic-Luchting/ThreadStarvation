# Thread starvation

This repository comes from a good presentation of Damian Edwards and David Fowler (two awesome guys from the .Net Core team) about Diagnosing issues in your code.

Simple story:

- One project is a little Hello world web application with 4 methods
  - Good old full synchronous execution, works but not very efficient
  - Modern app with legacy services: async-over-sync, not best but works
  - Old app code using modern apis: sync-over-async, shows dead IIS because of thread starvation
  - Full async, shows unlimited web scale

[Source code]  
https://github.com/davidfowl/NdcLondon2018

[Youtube]
https://www.youtube.com/watch?v=RYI0DHoIVaA

## Howto use it

- Open the repository folder in VS Code
- Open two terminal windows (CTRL+`)
- Navigate in the left to ~/Requestor
- Navigate in the right to ~/Threading 

Then you can `dotnet run` the web application in the right terminal and it shows Available threads and handled requests.
On the left side you can `dotnet run <method name>` where method name is one of the following
- hello
- hello-sync-over-async
- hello-async-over-sync
- hello-async

![VSC](https://github.com/nulllogicone/ThreadStarvation/blob/master/images/VS_Code.PNG)
