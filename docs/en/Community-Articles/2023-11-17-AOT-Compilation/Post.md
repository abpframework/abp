# Native AOT Compilation in .NET 8
Native AOT (Ahead-of-Time) compilation is a feature that allows developers to create a self-contained app compiled to native code that can run on machines without the .NET runtime installed. It results in benefits such as minimized disk footprint, reduced executable size, reduced startup time, and reduced memory demand.

Native AOT compilation isn't a new feature in .NET 8. It's first introduced in .NET 7.


Differences between the AOT Compilation of .NET 7 and .NET 8 are:


- **System.Text.Json improvements**: .NET 8 adds support for more types, source generation, interface hierarchies, naming policies, read-only properties, and more.
- **New types for performance**: .NET 8 introduces new types such as FrozenDictionary, FrozenSet, SearchValues, CompositeFormat, TimeProvider, and ITimer to improve the app performance.
- **System.Numerics and System.Runtime.Intrinsics enhancements**: .NET 8 adds support for Vector512, AVX-512, IUtf8SpanFormattable, Lerp, and more.
- **System.ComponentModel.DataAnnotations additions**: .NET 8 adds new data validation attributes for cloud-native services and a new ValidateOptionsResultBuilder type.
- **Hosted services lifecycle methods**: .NET 8 adds new methods such as StartAsync, StopAsync, StartBackgroundAsync, and StopBackgroundAsync for hosted services.

It's important to note that not all features in ASP.NET Core are currently compatible with native AOT. For more information, see [Native AOT deployment overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/).

## How to use Native AOT Compilation in .NET 8

You can add `<PublishAot>true</PublishAot>` in your project .csproj file to enable Native AOT Compilation.

  - For the new projects, you can create them with the `--aot` parameter. Example: `dotnet new console --aot`.

By default, the compiler chooses a blended approach code optimization but you can specify an optimization preference inside your .csproj file. You can choose **size** or **speed** according your requirements.

```xml
<OptimizationPreference>Size</OptimizationPreference>
```

or

```xml
<OptimizationPreference>Speed</OptimizationPreference>
```

### Results

I have created a simple console application to test the Native AOT Compilation. I have used a simple console application that writes "Hello World!" to the console 100 times. I have tested the application with different optimization preferences. I have used the following results:


|       | Size | Speed |
| ---   | ---   | ---  |
| .NET 8 <br/>_(Self-Contained, Single File)_   |  65938 kb     | 00.0051806  ~5ms   |
| .NET 7 AOT (default)          |   4452 kb     | 00.0029823  ~2ms |
| .NET 8 AOT (default)          |   1242 kb     | 00.0028638  ~2ms |
| AOT (Speed)| 1280 kb | 00.0023838  ~2ms |
| AOT (Size) | 1111 kb | 00.0025145  ~2ms |

Most of existing libraries don't support AOT compilation yet, so I couldn't use [BenchmarkDotnet](https://github.com/dotnet/BenchmarkDotNet) to measure the performance. I have used [Stopwatch](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?view=net-8.0) to measure the performance. So the performance results may not be accurate but gives insight about the performance difference.

## AOT Support in MAUI
You can now use Native AOT Compilation on iOS-like target frameworks in .NET MAUI. You can enable AOT compilation with the exact same method by adding `<PublishAot>true</PublishAot>` to your project .csproj file. According to the dotnet team, apps sizes reduced by 35% and startup times reduced by 28% with AOT compilation. And runtime performance is also improved by 50%.

But there are some limitations in MAUI AOT Compilation. A lot of libraries still don't support AOT compilation and some of platform-specific feaetures may not work at the moment.

## When to use Native AOT Compilation?

Native AOT Compilation is beneficial when you need to optimize your .NET application for speed and size. It's particularly useful for applications that require quick startup times and efficient runtime performance, such as mobile apps or high-performance computing applications.

However, due to its current limitations, it might not be suitable for all projects. If your project relies heavily on libraries that do not support AOT compilation, or if it uses platform-specific features that are not yet compatible with AOT, you might want to hold off on using Native AOT Compilation until further improvements are made.

Always consider the specific needs and constraints of your project before deciding to use Native AOT Compilation.

## Conclusion

Native AOT Compilation is a great feature that improves the performance of .NET applications. It's still in early-stages and not all libraries support it yet. But it's a great beginning for the future of .NET 🚀

## Links
- Native AOT deployment overview - .NET | Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/.
- Optimize AOT deployments https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/optimizing
- What's new in .NET 8 | Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8.

