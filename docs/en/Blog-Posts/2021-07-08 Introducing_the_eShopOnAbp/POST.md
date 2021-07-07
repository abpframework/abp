# Introducing the eShopOnAbp

We introduce the **eShopOnAbp** project as ABP Team. This application demonstrates strength of ABP Framework and using it in a real-life case. eShopOnAbp is a a full-featured cloud-native microservices reference application and is inspired by the [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers) project.

Currently, the project doesn't have any business logic and it's pure micro-service sample that we'll built business logic on.

The project has three types of end applications built with ASP.NET Core & Angular under ABP Framework:

- **Angular Application**: Back-Office application for management.
- **Public Web Application**: Landing page with end-user actions.
- **Backend Services**: There are some micro-services and gateways to perform backend processes

---

> Source code is available on  [GitHub | abpframework/eShopOnAbp](https://github.com/abpframework/eShopOnAbp)



## Structure

The project follows micro-service architecture and overall structure is presented below.

### Overall Solution

![eShopOnAbp Overall Solution](images/eShopOnAbp-Overall-Solution.png)



---



## How to run?

We're highly recommend to run application with tye. None of steps are required for infrastructure or running application. All steps are automated.

### Run with TYE

[Tye](https://github.com/dotnet/tye) is a developer tool that makes developing, testing, and deploying microservices and distributed applications easier.

 #### Requirements

- .NET 5.0+
- Docker
- Yarn

#### Instructions

- Clone the repository ( [eShopOnAbp](https://github.com/abpframework/eShopOnAbp) )

- Install TYE, follow [these steps](https://github.com/dotnet/tye/blob/main/docs/getting_started.md#installing-tye)

- Execute `run-tye.ps1`

- Wait until all applications are up!

  - You can check running application from tye dashboard ( [localhost:8000](http://127.0.0.1:8000/) )

- After all your backend services are up, start angular application:

  ```bash
  cd apps/angular
  yarn start



## Community

Your comments and suggestions is important for us. You can ask your questions or post your reviews under [eShopOnAbp Discussion](https://github.com/abpframework/abp/discussions/XXXX) page.



## What's next?

We'll work on deployment & CI-CD processes as a next step and build eShop business logic on.  First goal is deploying & hosting entire application on local kubernetes. 









