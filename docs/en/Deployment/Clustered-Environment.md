# Deploying to a Clustered Environment

This document introduces the topics that you should care when you are deploying your application to a clustered environment where **multiple instances of your application runs concurrently**, and explains how you can deal with these topics in your ABP based application.

> This document is valid regardless you have a monolith application or a microservice solution. The Application term is used for a process. An application can be a monolith web application, a service in a microservice solution, a console application, or another kind of executable process.

## Understanding the Clustered Environment

> You can skip this section if you are already familiar with clustered deployment and load balancers.

### Single Instance Deployment

Consider an application deployed as a **single instance**, as illustrated in the following figure:

![deployment-single-instance](../images/deployment-single-instance.png)

Browsers and other client applications can directly make HTTP requests to your application. You can put a web server (e.g. IIS or NGINX) between the clients and your application, but you still have a single application instance running in a single server or container. Single-instance configuration is **limited to scale** since it runs in a single server and you are limited with the server's capacity.

### Clustered Deployment

**Clustered deployment** is the way of running **multiple instances** of your application **concurrently** in a single or multiple servers. In this way, different instances can serve to different users or requests and you can scale by adding new servers to the system. The following figure shows a typical implementation of clustering using a **load balancer** service:

![deployment-clustered](../images/deployment-clustered.png)

### Load Balancers

[Load balancers](https://en.wikipedia.org/wiki/Load_balancing_(computing)) have a lot of features, but they fundamentally **forwards an incoming HTTP request** to an instance of your application and returns your response back to the client application.

Load balancers can use different algorithms for selecting the application instance while determining the application instance that is used to deliver the incoming request. **Round Robin** is one of the simplest and most used algorithms. Requests are delivered to the application instances in rotation. First instance gets the first request, second instance gets the second, and so on. It returns to the first instance after all the instances are used, and the algorithm goes like that for the next requests.

### Potential Problems

Once multiple instances of your application runs in parallel, you should carefully consider the following topics:

* Any **state (data) stored in memory** of your application will become a problem when you have multiple instances. A state stored in memory of an application instance may not be available in the next request since the next request will be handled by a different application instance. While there are some solutions (like sticky sessions) to overcome this problem user-basis, it is a **best practice to design your application as stateless** if you want to run it in a cluster, container or/and cloud.
* **In-memory caching** is a kind of in-memory state and should not be used in a clustered application. You should use **distributed caching** instead.
* You shouldn't store data in the **local file system** that should be available to all instances of your application. Difference application instance may run in different containers or servers and they may not be able to access to the same file system. You can use a **cloud or external storage provider** as a solution.
* If you have **background workers** or **job queue managers**, you should be careful since multiple instances may try to execute the same job or perform the same work. As a result, you may have the same work done multiple times or you may get a lot of errors while trying to access and change the same resource concurrently.

You may have more problems with clustered deployment, but these are the most common ones. ABP has been designed to be compatible with clustered deployment scenario. The following sections explains what you should do when you are deploying your ABP based application to a clustered environment.

## Switching to a Distributed Cache

TODO

## Using a Proper BLOB Storage Provider

TODO

## Configuring Background Jobs

TODO

## Implementing Background Workers

TODO

## Configuring a Distributed Lock Provider

TODO