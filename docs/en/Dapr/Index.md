# ABP Dapr Integration

> This document assumes that you are already familiar with [Dapr](https://dapr.io/) and you want to use it in your ABP based applications.

[Dapr](https://dapr.io/) (Distributed Application Runtime) provides APIs that simplify microservice connectivity. It is an open source project that is mainly backed by Microsoft. It is also a CNCF (Cloud Native Computing Foundation) project and trusted by community.

ABP and Dapr has some intersecting features like service-to-service communication, distributed message bus and distributed locking. However their purposes are totally different. ABP's goal is to provide an end-to-end developer experience by offering an opinionated architecture and providing the necessary infrastructure libraries, reusable modules and tools to implement that architecture properly. Dapr's purpose, on the other hand, is to provide a runtime to decouple common microservice communication patterns from your application logic.

ABP and Dapr can perfectly work together in the same application. You can just follow [its documentation](https://docs.dapr.io/) and use it as is. However, ABP offers some packages to provide better integration where Dapr features intersect with ABP. You can use other Dapr features with no ABP integration packages based on [its documentation](https://docs.dapr.io/). 

