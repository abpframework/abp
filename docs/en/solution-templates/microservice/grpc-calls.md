# Microservice Solution: gRPC Calls

````json
//[doc-nav]
{
  "Next": {
    "Name": "Distributed events in the Microservice solution",
    "Path": "solution-templates/microservice/distributed-events"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

You can use [gRPC](https://grpc.io) to enable high-performance, low-latency communication between microservices. gRPC, or Google Remote Procedure Call, is an open-source remote procedure call system initially developed by Google. It uses HTTP/2 for transport, Protocol Buffers as the interface description language, and provides features such as authentication, load balancing, and more.

For inter-service communication, gRPC is a great choice because it is faster and more efficient than REST. It is also language-agnostic, meaning you can use it with any programming language that supports gRPC. ABP does not restrict you to use gRPC; you can use it just like normal .NET applications.