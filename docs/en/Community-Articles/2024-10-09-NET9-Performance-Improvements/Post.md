# .NET 9 Performance Improvements Summary

With every release, .NET becomes faster & faster! You get these improvements for free by just updating your project to the latest .NET!

![Cover Image](cover.png)

It’s very interesting that **20% of these improvements** are implemented by **open-source volunteers** rather than Microsoft employees. These improvements mostly focus on cloud-native and high-throughput applications. I’ll briefly list them below.

![From Microsoft Blog Post](cited-from-microsoft-blog-post.png)



## 1. Dynamic PGO with JIT Compiler

*   ### What is dynamic PGO?
    With “Profile Guided Optimization” the compiler optimizes the code, based on the flow and the way the code executes. It is predicated on the idea that every potential behavior of the code will always transpire.

*   ### What’s Improved?
    The tiered compilation, inlining, and dynamic PGO are three ways that .NET 9 optimizes the JIT compiler. This enhances runtime performance and speeds up the time for apps to launch.

*   ### Performance Gains
    CPU use is lower during execution; therefore, **startup times are about 15% faster**.

*   ### As a Developer
    Faster, smoother deployments with reduced warm-up times... These enhancements reduce latency for applications with complex workflows, particularly in microservices and high-throughput environments.

*   ### How to activate Dynamic PGO?
    Add the following to your `csproj` file, or if you have several `csproj` files, you can add it once in `Directory.Build.props` file. Check out [this link](https://learn.microsoft.com/en-us/dotnet/core/runtime-config/compilation#profile-guided-optimization) to understand PGO.

```xml
  <PropertyGroup>
    <TieredPGO>true</TieredPGO>
  </PropertyGroup>
```



## 2. Library Improvements

*   ### What’s Improved?
    
    LINQ and JSON serialization, collections and libraries are significantly improved with .NET 9.
    
*   ### Performance Gains
    
    **JSON serialization** performance **increases by about 35%**. This helps with heavy data parsing and API requests. Less memory is allocated to `Span` operations as well, and LINQ techniques such as `Where` and `Select` are now faster.
    
*   ### As a Developer
    
    This means that apps will be faster, especially those that handle data primarily in JSON or manipulate data with LINQ.



## 3. ASP.NET Core

*   ### What’s Improved?
    Kestrel server has undergone significant modifications, mostly in processing the HTTP/2 and HTTP/3 protocols.
    
*   ### Performance Gains
    Now, **Kestrel handles requests up to 20% faster** and **has a 25% reduction in average latency**. Improved connection management and SSL processing also result in overall efficiency gains.
    
*   ### As a Developer
    These modifications result in less resource use, quicker response times for web applications, and more seamless scaling in high-traffic situations.



## 4. Garbage Collection & Memory Management

*   ### What’s Improved?
    NET 9’s garbage collection (GC) is more effective, especially for apps with high allocation rates.
    
*   ### Performance Gains
    Applications experience smoother **garbage collection cycles with 8–12% less memory overhead**, which lowers latency and delays.
    
*   ### As a Developer
    The performance will be more reliable and predictable for developers as there will be fewer memory-related bottlenecks, particularly in applications that involve frequent object allocations.



## 5. Native AOT Compilation

*   ### What’s Improved?
    Native AOT (Ahead-of-Time) compilation is now more efficient by lowering memory footprint and cold-start times. This leads to better support for cloud-native applications.
    
*   ### Performance Gains
    Native AOT apps now have faster cold launches and use **30–40% less memory**. This improvement focuses on containerized applications.

---



**References:**

*   [Microsoft .NET blog post](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-9/).
*   [What’s new in the .NET 9 runtime?](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9/runtime#performance-improvements)

