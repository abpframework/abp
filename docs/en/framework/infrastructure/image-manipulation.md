# Image Manipulation
ABP provides services to compress and resize images and implements these services with popular [ImageSharp](https://sixlabors.com/products/imagesharp/) and [Magick.NET](https://github.com/dlemstra/Magick.NET) libraries. You can use these services in your reusable modules, libraries and applications, so you don't depend on a specific imaging library.

> The image resizer/compressor system is designed to be extensible. You can implement your own image resizer/compressor contributor and use it in your application.

## Installation

You can add this package to your application by either using the [ABP CLI](../../cli) or manually installing it. Using the [ABP CLI](../../cli) is the recommended approach.

### Using the ABP CLI

Open a command line terminal in the folder of your project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Imaging.Abstractions
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Imaging.Abstractions](https://www.nuget.org/packages/Volo.Abp.Imaging.Abstractions) NuGet package to your project:

```
Install-Package Volo.Abp.Imaging.Abstractions
```

2. Add the `AbpImagingAbstractionsModule` to the dependency list of your module:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpImagingAbstractionsModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
```

## Providers

ABP provides two image resizer/compressor implementations out of the box:

* [Magick.NET](#magick-net-provider)
* [ImageSharp](#imagesharp-provider)

You should install one of these provides to make it actually working.

> If none of the provider packages installed into your application, compress/resize operations return the untouched input image.

## IImageResizer

You can [inject](../fundamentals/dependency-injection.md) the `IImageResizer` service and use it for image resize operations. Here is the available methods of the `IImageResizer` service:

```csharp
public interface IImageResizer
{
    /* Works with a Stream object that represents an image */
    Task<ImageResizeResult<Stream>> ResizeAsync(
        Stream stream,
        ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default
    );

    /* Works with a byte array that contains an image file */
    Task<ImageResizeResult<byte[]>> ResizeAsync(
        byte[] bytes,
        ImageResizeArgs resizeArgs,
        string mimeType = null,
        CancellationToken cancellationToken = default
    );
}
```

**Example usage:**

```csharp
var resizeResult = await _imageResizer.ResizeAsync(
    imageStream, /* A stream object that represents an image */
    new ImageResizeArgs
    {
        Width = 100,
        Height = 100,
        Mode = ImageResizeMode.Crop
    },
    mimeType: "image/jpeg"
);
```

> **Note:** If `resizeResult.State` returns 'Done', then it means that the resize operation was successful. However, if it returns any other state than 'Done', the stream you're using might be corrupted. Therefore, you can perform a check like the one below and assign the correct stream to the main stream:

```csharp
if (resizeResult.Result is not null && imageStream != resizeResult.Result && resizeResult.Result.CanRead)
{
    await imageStream.DisposeAsync();
    imageStream = resizeResult.Result;
}
```

> You can use `MimeTypes.Image.Jpeg` constant instead of the `image/jpeg` magic string used in that example.

### ImageResizeArgs

The `ImageResizeArgs` is a class that is used to define the resize operation parameters. It has the following properties:

* `Width`: The width of the resized image.
* `Height`: The height of the resized image.
* `Mode`: The resize mode (see the [ImageResizeMode](#imageresizemode) section for more information).

### ImageResizeMode

The `ImageResizeMode` is an enum that is used to define the resize mode. It has the following values:

```csharp
public enum ImageResizeMode : byte
{
    None = 0,
    Stretch = 1,
    BoxPad = 2,
    Min = 3,
    Max = 4,
    Crop = 5,
    Pad = 6,
    Default = 7
}
```

> See the [ImageSharp documentation](https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Processing.ResizeMode.html) for more information about the resize modes.

### ImageResizeResult

The `ImageResizeResult` is a generic class that is used to return the result of the image resize operations. It has the following properties:

* `Result`: The resized image (stream or byte array).
* `State`: The result of the resize operation (type: `ImageProcessState`).

### ImageProcessState

The `ImageProcessState` is an enum that is used to return the the result of the image resize operations. It has the following values:

```csharp
public enum ImageProcessState : byte
{
    Done = 1,
    Canceled = 2,
    Unsupported = 3,
}
```

### ImageResizeOptions

`ImageResizeOptions` is an [options object](../fundamentals/options.md) that is used to configure the image resize system. It has the following properties:

* `DefaultResizeMode`: The default resize mode. (Default: `ImageResizeMode.None`)

## IImageCompressor

You can [inject](../fundamentals/dependency-injection.md) the `IImageCompressor` service and use it for image compression operations. Here is the available methods of the `IImageCompressor` service:

```csharp
public interface IImageCompressor
{
    /* Works with a Stream object that represents an image */
    Task<ImageCompressResult<Stream>> CompressAsync(
        Stream stream,
        string mimeType = null,
        CancellationToken cancellationToken = default
    );
    
    /* Works with a byte array that contains an image file */
    Task<ImageCompressResult<byte[]>> CompressAsync(
        byte[] bytes,
        string mimeType = null,
        CancellationToken cancellationToken = default
    );
}
```

**Example usage:**

```csharp
var compressResult = await _imageCompressor.CompressAsync(
    imageStream, /* A stream object that represents an image */
    mimeType: "image/jpeg"
);
```

> **Note:** If `compressResult.State` returns 'Done', then it means that the compression operation was successful. However, if it returns any other state than 'Done', the stream you're using might be corrupted. Therefore, you can perform a check like the one below and assign the correct stream to the main stream:

```csharp

if (compressResult.Result is not null && imageStream != compressResult.Result && compressResult.Result.CanRead)
{
    await imageStream.DisposeAsync();
    imageStream = compressResult.Result;
}
```

### ImageCompressResult

The `ImageCompressResult` is a generic class that is used to return the result of the image compression operations. It has the following properties:

* `Result`: The compressed image (stream or byte array).
* `State`: The result of the compress operation (type: `ImageProcessState`).

### ImageProcessState

The `ImageProcessState` is an enum that is used to return the the result of the image compress operations. It has the following values:

```csharp
public enum ImageProcessState : byte
{
    Done = 1,
    Canceled = 2,
    Unsupported = 3,
}
```

## Magick.NET Provider

`Volo.Abp.Imaging.MagickNet` NuGet package implements the image operations using the [Magick.NET](https://github.com/dlemstra/Magick.NET) library.

## Installation

You can add this package to your application by either using the [ABP CLI](../../cli) or manually installing it. Using the [ABP CLI](../../cli) is the recommended approach.

### Using the ABP CLI

Open a command line terminal in the folder of your project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Imaging.MagickNet
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Imaging.MagickNet](https://www.nuget.org/packages/Volo.Abp.Imaging.MagickNet) NuGet package to your project:

```
Install-Package Volo.Abp.Imaging.MagickNet
```

2. Add `AbpImagingMagickNetModule` to your [module](../architecture/modularity/basics.md)'s dependency list:

```csharp
[DependsOn(typeof(AbpImagingMagickNetModule))]
public class MyModule : AbpModule
{
    //...
}
```

### Configuration

`MagickNetCompressOptions` is an [options object](../fundamentals/options.md) that is used to configure the Magick.NET image compression system. It has the following properties:

* `OptimalCompression`: Indicates whether the optimal compression is enabled or not. (Default: `false`)
* `IgnoreUnsupportedFormats`: Indicates whether the unsupported formats are ignored or not. (Default: `false`)
* `Lossless`: Indicates whether the lossless compression is enabled or not. (Default: `false`)

## ImageSharp Provider

`Volo.Abp.Imaging.ImageSharp` NuGet package implements the image operations using the [ImageSharp](https://github.com/SixLabors/ImageSharp) library. 

## Installation

You can add this package to your application by either using the [ABP CLI](../../cli) or manually installing it. Using the [ABP CLI](../../cli) is the recommended approach.

### Using the ABP CLI

Open a command line terminal in the folder of your project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Imaging.ImageSharp
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Imaging.ImageSharp](https://www.nuget.org/packages/Volo.Abp.Imaging.ImageSharp) NuGet package to your project:

```
Install-Package Volo.Abp.Imaging.ImageSharp
```

2. Add `AbpImagingImageSharpModule` to your [module](../architecture/modularity/basics.md)'s dependency list:


```csharp
[DependsOn(typeof(AbpImagingImageSharpModule))]
public class MyModule : AbpModule
{
    //...
}
```

### Configuration

`ImageSharpCompressOptions` is an [options object](../fundamentals/options.md) that is used to configure the ImageSharp image compression system. It has the following properties:

* `DefaultQuality`: The default quality of the JPEG and WebP encoders. (Default: `75`)
* [`JpegEncoder`](https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder.html): The JPEG encoder. (Default: `JpegEncoder` with `Quality` set to `DefaultQuality`)
* [`PngEncoder`](https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Formats.Png.PngEncoder.html): The PNG encoder. (Default: `PngEncoder` with `IgnoreMetadata` set to `true` and `CompressionLevel` set to `PngCompressionLevel.BestCompression`)
* [`WebPEncoder`](https://docs.sixlabors.com/api/ImageSharp/SixLabors.ImageSharp.Formats.Webp.WebpEncoder.html): The WebP encoder. (Default: `WebPEncoder` with `Quality` set to `DefaultQuality`)

**Example usage:**
    
```csharp
Configure<ImageSharpCompressOptions>(options =>
{
    options.JpegEncoder = new JpegEncoder
    {
        Quality = 60
    };
    options.PngEncoder = new PngEncoder
    {
        CompressionLevel = PngCompressionLevel.BestCompression
    };
    options.WebPEncoder = new WebPEncoder
    {
        Quality = 65
    };
});
```

## ASP.NET Core Integration

`Volo.Abp.Imaging.AspNetCore` NuGet package defines attributes for controller actions that can automatically compress and/or resize uploaded files.

## Installation

You can add this package to your application by either using the [ABP CLI](../../cli) or manually installing it. Using the [ABP CLI](../../cli) is the recommended approach.

### Using the ABP CLI

Open a command line terminal in the folder of your project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Imaging.AspNetCore
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Imaging.AspNetCore](https://www.nuget.org/packages/Volo.Abp.Imaging.AspNetCore) NuGet package to your project:

```
Install-Package Volo.Abp.Imaging.AspNetCore
```

2. Add `AbpImagingAspNetCoreModule` to your [module](../architecture/modularity/basics.md)'s dependency list:

```csharp
[DependsOn(typeof(AbpImagingAspNetCoreModule))]
public class MyModule : AbpModule
{
    //...
}
```

### CompressImageAttribute

The `CompressImageAttribute` is used to compress the image before. `IFormFile`, `IRemoteStreamContent`, `Stream` and `IEnumrable<byte>` types are supported. It has the following properties:

* `Parameters`: Names of the the parameters that are used to configure the image compression system. This is useful if your action has some non-image parameters. If you don't specify the parameters names, all of the method parameters are considered as image.

**Example usage:**

```csharp
[HttpPost]
[CompressImage] /* Compresses the given file (automatically determines the file mime type) */
public async Task<IActionResult> Upload(IFormFile file)
{
    //...
}
```

### ResizeImageAttribute

The `ResizeImageAttribute` is used to resize the image before requesting the action. `IFormFile`, `IRemoteStreamContent`, `Stream` and `IEnumrable<byte>` types are supported. It has the following properties:

* `Parameters`: Names of the the parameters that are used to configure the image resize system. This is useful if your action has some non-image parameters. If you don't specify the parameters names, all of the method parameters are considered as image.
* `Width`: Target width of the resized image.
* `Height`: Target height of the resized image.
* `Mode`: The resize mode (see the [ImageResizeMode](#imageresizemode) section for more information).

**Example usage:**

```csharp
[HttpPost]
[ResizeImage(Width = 100, Height = 100, Mode = ImageResizeMode.Crop)] 
public async Task<IActionResult> Upload(IFormFile file)
{
    //...
}
```
