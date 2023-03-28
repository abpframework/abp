# What’s New in .NET 8 🧐 ? Discover ALL .NET 8 Features⚡🚀

In this post, I'll briefly mention the new features of .NET 8 and the changes.

## `dotnet publish` and `dotnet pack` Release Mode 🏭

With this new version, `dotnet publish` and `dotnet pack` commands will build and pack with the `Release` mode. Before it was producing in `Debug` mode. To be able to produce in Debug mode, you need to set this parameter `-p:PublishRelease` as false.

```bash
dotnet publish -> /app/bin/Release/net8.0/app.dll
dotnet publish -p:PublishRelease=false -> /app/bin/Debug/net8.0/app.dll
```

---



## System.Text.Json Serialization 🧱

[System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json) replaced Newtonsoft.Json in the recent versions. We are also using `System.Text.Json` in the [ABP Framework](https://abp.io) now. There are several enhancements to object serialization and deserialization.

The latest version of the [source generator](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation) now offers improved  performance and reliability for Native AOT apps when used with ASP.NET  Core. It also allows serializing types with [`required`](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/required-properties) and [`init`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init) properties already supported in reflection-based  serialization. Additionally, there is now an option to customize the  handling of members that are not present in the JSON payload, see https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/missing-members. Support for serializing properties from interface hierarchies. The  [JsonNamingPolicy](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonnamingpolicy?view=net-8.0&preserve-view=true#properties) feature has been expanded to include new naming  policies for `snake_case` and `kebab-case` property name conversions.  Finally, [JsonSerializerOptions.MakeReadOnly](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.makereadonly#system-text-json-jsonserializeroptions-makereadonly) method allows for explicit  control over when a `JsonSerializerOptions` instance is frozen, and you  can check its status using the [IsReadOnly](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.isreadonly#system-text-json-jsonserializeroptions-isreadonly)  property.

---



## Randomness

AI programming is very popular these days. And the need to produce more random content arose. 

### GetItems<T>() 🧮

Two new methods: [Random.GetItems](https://learn.microsoft.com/en-us/dotnet/api/system.random.getitems) and [RandomNumberGenerator.GetItems](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator.getitems) have been introduced that enable developers to randomly select a set number  of items from a given input set. The example below demonstrates the  usage of the `System.Random.GetItems<T>()` method using an instance obtained from the `Random.Shared` property to randomly insert 31 items into an array.

```csharp
private static ReadOnlySpan<CountryPhoneCodePhoneCode> countries = new[]
{
    new CountryPhoneCode("Turkey", "90"),
    new CountryPhoneCode("China", "86"),
    new CountryPhoneCode("Germany", "49"),
    new CountryPhoneCode("Finland", "358"),
    new CountryPhoneCode("Spain", "34")
};

var randomValues = Random.Shared.GetItems(countries, 2);
foreach (var x in randomValues)
{
    Console.WriteLine(x.Name + " -> " + x.CountryPhoneCode);
}

/**************
- Output -
Germany -> 49
Finland -> 358
**************/
```

---



### Shuffle<T>() 🔀

If you need to randomize the order of a span in your application, you can take advantage of two new methods: [Random.Shuffle](https://learn.microsoft.com/en-us/dotnet/api/system.random.shuffle) and [RandomNumberGenerator.Shuffle](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator.shuffle?view=net-8.0). These methods are particularly handy when you want to minimize the impact of training bias in machine learning by varying the order in which training and testing data are presented. Using these methods, you can ensure that the first thing in your dataset is only sometimes used for training, and the last is only sometimes reserved for testing.

```csharp
var trainingData = GetData();
Random.Shared.Shuffle(trainingData);

IDataView source = mlContext.Data.LoadFromEnumerable(trainingData);

DataOperationsCatalog.TrainTestData splittedData = mlContext.Data.TrainTestSplit(source);
model = chain.Fit(splittedData.TrainSet);

IDataView resultPredictions = model.Transform(split.TestSet);
```

---



## Performance Improvements 🚀

In .NET 8, various new types have been introduced to enhance application performance.

- The [System.Collections.Frozen](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen) namespace in .NET 8 includes the [FrozenDictionary](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2) and [FrozenSet](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozenset-1) collection types. These types are designed to prevent changes to keys  and values once a collection is created, resulting in faster read  operations such as `TryGetValue()`. They are particularly useful for  collections populated on first use and then persisted for a long-lived service.

    ```csharp
    private static readonly FrozenDictionary<string, bool> frozenData = LoadConfigurationData().ToFrozenDictionary(optimizeForReads: true); 
    //////
    if (frozenData.TryGetValue(key, out bool setting) && setting) 
    {
    Process();
    }
    ```

- [Buffers.IndexOfAnyValues](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.indexofanyvalues-1) is a new type in .NET 8, designed to be passed to methods that search for the first occurrence of any value in a passed collection. The new overloads of methods like [String.IndexOfAny](https://learn.microsoft.com/en-us/dotnet/api/system.string.indexofany?view=net-8.0#system-string-indexofany(system-char())) and [MemoryExtensions.IndexOfAny](https://learn.microsoft.com/en-us/dotnet/api/system.memoryextensions.indexofany) accept an instance of the new type. When you create an instance of [Buffers.IndexOfAnyValues](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.indexofanyvalues-1), all the necessary data for optimizing subsequent searches is derived at that time.

- [Text.CompositeFormat](https://learn.microsoft.com/en-us/dotnet/api/system.text.compositeformat) is a new type in .NET 8 useful for optimizing format strings that aren't known at compile time (such as format strings loaded from a resource file). While some extra time is spent upfront to perform tasks like parsing the string, it saves the work from being done each time the format string is used.

    ```csharp
    private static readonly CompositeFormat range = CompositeFormat.Parse(Load());
    //////////
    static string GetMessage(int min, int max) =>
        string.Format(CultureInfo.InvariantCulture, range, min, max);
    ```

- In .NET 8, two new types are introduced to implement the fast  [XxHash3](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.xxhash3) and [XxHash128](https://learn.microsoft.com/en-us/dotnet/api/system.io.hashing.xxhash128) hash algorithms.

---



## Improvements in System.Numerics  and System.Runtime.Intrinsics 🔥

There are several enhancements made to the  [System.Numerics](https://learn.microsoft.com/en-us/dotnet/api/system.numerics) and [System.Runtime.Intrinsics](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics)  namespaces. These improvements include better  hardware acceleration for [Vector256](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector256-1), [Matrix3x2](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.matrix3x2), and [Matrix4x4](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.matrix4x4)  in .NET 8. 

[Vector256](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector256-1) was redesigned to utilize `2x Vector128<T>` operations internally to achieve partial acceleration of certain  functions on `Arm64` processors where `Vector128.IsHardwareAccelerated == true` but `Vector256.IsHardwareAccelerated == false`. The introduction of  [Vector512](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector512-1) is also included in .NET 8. 

Additionally, the  `ConstExpected` attribute has been added to hardware intrinsic to alert  users when a non-constant value might cause unexpected performance  issues. 

Lastly, the [Lerp(TSelf, TSelf, TSelf)](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpointieee754-1.lerp#system-numerics-ifloatingpointieee754-1-lerp(-0-0-0)) API has been added to  [IFloatingPointIeee754](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpointieee754-1), enabling the efficient and accurate linear interpolation of two values in `float`([Single](https://learn.microsoft.com/en-us/dotnet/api/system.single)), `double` ([Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)), and [Half](https://learn.microsoft.com/en-us/dotnet/api/system.half).

---



## New Data Validation Attributes 🛡️

The [DataAnnotations](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations) namespace, aimed specifically for  validation in cloud-native services. The existing `DataAnnotations` validators are primarily used for validating user data, like form fields. However, the new attributes are meant to validate data, not entered by users, like [configuration options](https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-validation). Apart from the new attributes, the [RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute) and [RequiredAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.requiredattribute) types also received new properties.

- [RequiredAttribute.DisallowAllDefaultValues](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.requiredattribute.disallowalldefaultvalues#system-componentmodel-dataannotations-requiredattribute-disallowalldefaultvalues): The attribute forces that structs for inequality with their default values.
- [RangeAttribute.MinimumIsExclusive](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute.minimumisexclusive#system-componentmodel-dataannotations-rangeattribute-minimumisexclusive) & [RangeAttribute.MaximumIsExclusive](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute.maximumisexclusive#system-componentmodel-dataannotations-rangeattribute-maximumisexclusive): Specifies whether the allowable range includes its boundaries or not.
- [DataAnnotations.LengthAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.lengthattribute): Specifies the lower and upper limits for strings or collections using the `Length` attribute. For instance, the `[Length(5, 100)]` attribute specifies that a collection must have at least 5 elements and at most 100 elements.
- [DataAnnotations.Base64StringAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.base64stringattribute): Validates a valid `Base64` format.
- [DataAnnotations.AllowedValuesAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.allowedvaluesattribute) & [DataAnnotations.DeniedValuesAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.deniedvaluesattribute): Specifies accepted allow lists or not allowed deny lists. For instance: `[AllowedValues("red", "green", "blue")]` or `[DeniedValues("yellow", "purple")]`.



---



## Function Pointers Introspection Support ↩️

[Function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#function-pointers) were released with .NET 5. There was no support for reflection at that time. As a result, using `typeof` or reflection on a function pointer, such as `typeof(delegate*<void>())` or `FieldInfo.FieldType`, respectively, would return an [IntPtr](https://learn.microsoft.com/en-us/dotnet/api/system.intptr). However, in .NET 8, a [System.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type) object is returned instead, providing access to function pointer  metadata, such as calling conventions, return type, and parameters. This functionality is implemented only in the `CoreCLR` runtime and [MetadataLoadContext](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.metadataloadcontext).

---



## Native AOT 🏭

The [publishing as native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) was initially introduced in .NET 7, the option to publish an application as native AOT enables the creation of a self-contained version of the app that does not require a separate runtime, bundling everything into a single file.

In .NET 8, the support for native AOT now encompasses the `x64` and `Arm64` architectures on macOS. Moreover, native AOT applications on Linux are now up to 50% smaller in size. Here's the table, illustrates the size of a minimal app published with native AOT, containing the entire .NET runtime:



* **Linux x64** (with `-p:StripSymbols=true`) 
  * .NET 7 ➡ 3.76MB
  * .NET 8 ➡ 1.84 MB
* **Windows x64**
  * .NET 7 ➡ 2.85 MB
  * .NET 8 ➡ 1.77 MB 

---



## Code Generation Improvements 📃

.NET 8 includes enhancements to code generation and just-in-time (*JIT*) compilation:

- JIT throughput improvements
- Arm64 performance improvements
- Profile-guided optimization (PGO) improvements
- Support for AVX-512 ISA extensions
- SIMD improvements
- Cloud-native improvements
- Loop and general optimizations

---



## .NET 8 DevOps Improvements 📦

### NET Container Image Changes 

There are some changes with .NET 8 on image containers. First, [Debian 12](https://wiki.debian.org/DebianBookworm) is the **default Linux distribution** in the container images.

Secondly, the images include a `non-root` user to make the images `non-root` capable. To run as `non-root`, add the line `USER app` at the end of your `Dockerfile`.

Besides, the **default port has also changed** from `80` to `8080` and a **new environment variable** `ASPNETCORE_HTTP_PORTS` is available to change ports easily.

Also, the format for the `ASPNETCORE_HTTP_PORTS` variable is easier compared to the format required by `ASPNETCORE_URLS`, and it accepts a list of ports. If you change the port back to `80` using one of these variables, it won’t be possible to run as `non-root`.

Finally, .NET 8 is now supported on **Chiseled Ubuntu** images, available at the [Ubuntu/DotNet-deps Docker Hub](Ubuntu/DotNet-deps Docker Hub). Chiseled images are  designed to have a smaller attack surface as they are stripped down to  be ultra-compact, and do not include a package manager or shell.  Chiseled images are non-root, making them ideal for developers looking for the benefits of appliance-style computing. These  images are regularly published to the [.NET nightly artifact registry](https://mcr.microsoft.com/product/dotnet/nightly/aspnet/tags) for easy access.

### Building Your .NET on Linux 

Previously, building .NET from source in earlier versions required  creating a `source tarball` from the corresponding release commit in the [dotnet/installer repository](dotnet/installer repository). However, in .NET 8, this step is no longer necessary as the [dotnet/dotnet repository](https://github.com/dotnet/dotnet)  allows building .NET directly on Linux using  [dotnet/source-build](https://github.com/dotnet/source-build) to create runtimes, tools, and SDKs. Red Hat and Canonical also use this build for .NET. Building in a container is the easiest approach for most people since the `dotnet-buildtools/prereqs` container images have all the necessary dependencies. [The build instructions]() provide more information.

### Minimum support baselines for Linux

The support requirements for Linux have been updated for .NET 8, with changes to the minimum support baselines:

1. All architectures will target Ubuntu 16.04 for building .NET, which is important for setting the minimum required version of `glibc` for .NET 8. Versions of Ubuntu earlier than 16.04, such as 14.04, will not even allow .NET 8 to start.
2. **Red Hat Enterprise Linux 7 is no longer supported** with .NET 8.  Only supporting RHEL 8 and later.

For further details, please refer to the [support for Red Hat Enterprise Linux Family](https://github.com/dotnet/core/blob/main/linux-support.md#red-hat-enterprise-linux-family-support) page.



---

Become a pioneer and try the new features of .NET 8 now. 
Adapt it to your project or start a new .NET 8 project. 
[Claim your copy of .NET 8](https://dotnet.microsoft.com/next) today 🏎️ !

〰️〰️〰️

Happy Coding ⌨️

