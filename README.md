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