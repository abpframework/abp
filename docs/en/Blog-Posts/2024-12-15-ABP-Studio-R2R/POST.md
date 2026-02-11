# ABP Studio Goes AOT: Faster Startups with Ready-to-Run (R2R) Publishing

We're excited that [ABP Studio](https://abp.io/studio) now supports [Ready-to-Run (R2R) publishing](https://learn.microsoft.com/en-us/dotnet/core/deploying/ready-to-run) (starting from v0.9.16+), a hybrid form of ahead-of-time (AOT) compilation. This enhancement significantly improves the startup time and overall performance of ABP Studio, making it faster and more performant than ever before.

Let's dive into what R2R publishing is, how it works, and the benefits it brings to ABP Studio.

## What is Ready-to-Run (R2R) Publishing?

Ready-to-Run (R2R) is a form of AOT compilation available in the .NET ecosystem. Unlike traditional just-in-time (JIT) compilation, R2R precompiles parts of your application to native code before deployment. This precompiled code helps reduce the startup time by minimizing the work needed during runtime.

However, R2R isn't a complete AOT compilation. Instead, it's a hybrid approach because it stores both:

* **Native code for precompiled methods** (to improve startup time and performance)

* **Intermediate Language (IL) code** for methods that may need further JIT compilation

This hybrid nature is why R2R binaries are typically larger. For ABP Studio, the storage size increased by ~150 MB with R2R enabled, but the trade-off is well worth it for the performance and startup-time gains.

## How R2R (Ready-to-Run) Improves ABP Studio

### Faster Startup Time 🚀

One of the biggest advantages of R2R publishing is its impact on startup times. In our local tests, enabling R2R resulted in startup times being **reduced by 2.5x** ⬇️.

This means you can get to work faster, without waiting for the application to being startup from the beginning. Whether you're launching ABP Studio to manage projects, generate code, or deploy applications, the improved responsiveness is noticeable.

### Performance Enhancements 📈

In addition to faster startups, R2R publishing contributes to overall performance improvements. By precompiling frequently used methods, R2R reduces the workload on the JIT compiler during execution, leading to smoother and more efficient operations.

### Trade-offs: Increased Storage Size 🆙

With great performance comes a slight trade-off: storage size. R2R binaries include both **native** and **IL code**, which increases the file size. In the case of ABP Studio, the storage footprint increased by ~150 MB. However, the substantial improvements in speed and responsiveness make this a worthwhile investment.

## How to Enable R2R Publishing in Your Applications?

If you're developing applications and want to benefit from R2R, here's a quick guide on how to enable it in your .NET projects:

1. You can add the following configuration to your final project's `.csproj` file:

```xml
<PropertyGroup>
    <PublishReadyToRun>true</PublishReadyToRun>
</PropertyGroup>
```

2. Then, publish your application with the `dotnet publish` command:

```bash
dotnet publish -c Release
```

Alternatively, you can specify the _PublishReadyToRun_ flag directly to the `dotnet publish` command as follows:

```bash
dotnet publish -c Release -r win-x64 -p:PublishReadyToRun=true
```

That's it! Your application will now include precompiled native code for faster startup and great performance benefits.

> Please refer to the [official documentation](https://learn.microsoft.com/en-us/dotnet/core/deploying/ready-to-run) before publishing your application with R2R.

## Conclusion

As ABP team, we're always looking for ways to improve the developer experience. By adopting **Ready-to-Run (R2R) publishing** for ABP Studio, we're aiming to deliver a faster and more efficient tool for your development needs.

Stay tuned for more updates and enhancements as we continue to optimize ABP Studio and please provide us with your invaluable feedback.