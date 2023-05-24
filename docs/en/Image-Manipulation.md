# Image Manipulation
The ABP Framework provides an abstraction to work with image compress/resize. Having such an abstraction has some benefits;

* You can write library independent code. Therefore, you can change the underlying library with the minimum effort and code change.
* You can use the predefined image resizer/compressor defined in the ABP without worrying about the underlying library's internal details.

> The image resizer/compressor system is designed to be extensible. You can implement your own image resizer/compressor contributor and use it in your application.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

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

The ABP Framework provides two image resizer/compressor providers:

* [Magick.NET](#magicknet)
* [ImageSharp](#imagesharp)

> It needs a provider to work. You can use the [Magick.NET](#magicknet), [ImageSharp](#imagesharp) provider or implement your own provider. Provider does not exist, the input stream will be returned as it is.

## IImageResizer

You can inject `IImageResizer` and use it for image resize operations. Here is the available operations in the `IImageResizer` interface.

```csharp
public interface IImageResizer
{
    Task<ImageResizeResult<Stream>> ResizeAsync(Stream stream, ImageResizeArgs resizeArgs, [CanBeNull]string mimeType = null, CancellationToken cancellationToken = default);
    
    Task<ImageResizeResult<byte[]>> ResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, [CanBeNull] string mimeType = null, CancellationToken cancellationToken = default);
}
```

Usage example:

```csharp
var result = await _imageResizer.ResizeAsync(
    stream,
    new ImageResizeArgs
    {
        Width = 100,
        Height = 100,
        Mode = ImageResizeMode.Crop
    },
    mimeType: "image/jpeg"
);
```

## ImageResizeArgs

The `ImageResizeArgs` is a class that is used to define the resize operation parameters. It has the following properties:

* `Width`: The width of the resized image.
* `Height`: The height of the resized image.
* `Mode`: The resize mode. (See the [ImageResizeMode](#imageresizemode) section for more information.)

## ImageResizeMode

The `ImageResizeMode` is an enum that is used to define the resize mode. It has the following values:

```csharp
public enum ImageResizeMode
{
    None,
    Stretch,
    BoxPad,
    Min,
    Max,
    Crop,
    Pad,
    Default
}
```

## IImageResizerContributor

The `IImageResizerContributor` is an interface that is used to implement a custom image resizer. It has the following method:

```csharp
public interface IImageResizerContributor
{
    Task<ImageResizeResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
    
    Task<ImageResizeResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
}
```

## ImageResizeResult

The `ImageResizeResult` is a generic class that is used to return the result of the image resize operations. It has the following properties:

* `Result`: The resized image stream or byte array.
* `State`: The state of the resize operation. (See the [ProcessState](#processstate) section for more information.)

## IImageCompressor

You can inject `IImageCompressor` and use it for image compression operations. Here is the available operations in the `IImageCompressor` interface.

```csharp
public interface IImageCompressor
{
    Task<ImageCompressResult<Stream>> CompressAsync(
        Stream stream,
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default);
    
    Task<ImageCompressResult<byte[]>> CompressAsync(
        byte[] bytes,
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default);
}
```

Usage example:

```csharp
var result = await _imageCompressor.CompressAsync(
    stream,
    mimeType: "image/jpeg"
);
```

## ImageCompressResult

The `ImageCompressResult` is a generic class that is used to return the result of the image compression operations. It has the following properties:

* `Result`: The compressed image stream or byte array.
* `State`: The state of the compression operation. (See the [ProcessState](#processstate) section for more information.)

## IImageCompressorContributor

The `IImageCompressorContributor` is an interface that is used to implement a custom image compressor. It has the following method:

```csharp
public interface IImageCompressorContributor
{
    Task<ImageCompressResult<Stream>> TryCompressAsync(Stream stream, string mimeType = null, CancellationToken cancellationToken = default);
    Task<ImageCompressResult<byte[]>> TryCompressAsync(byte[] bytes, string mimeType = null, CancellationToken cancellationToken = default);
}
```

## ProcessState

The `ProcessState` is an enum that is used to define the state of the image resize/compression operations. It has the following values:

```csharp
public enum ProcessState : byte
{
    Done = 1,
    Canceled = 2,
    Unsupported = 3,
}
```

## Configuration

### ImageResizeOptions

`ImageResizeOptions` is used to configure the image resize system. It has the following properties:

* `DefaultResizeMode`: The default resize mode. (Default: `ImageResizeMode.None`)

## [Magick.NET](https://github.com/dlemstra/Magick.NET)

Add the `Volo.Abp.Imaging.MagickNet` NuGet package to your project and use the `AbpImagingMagickNetModule` module.

```csharp
[DependsOn(typeof(AbpImagingMagickNetModule))]
public class MyModule : AbpModule
{
    //...
}
```

### Configuration

#### MagickNetCompressOptions

`MagickNetCompressOptions` is used to configure the Magick.NET image compression system. It has the following properties:

* `OptimalCompression`: Indicates whether the optimal compression is enabled or not. (Default: `false`)
* `IgnoreUnsupportedFormats`: Indicates whether the unsupported formats are ignored or not. (Default: `false`)
* `Lossless`: Indicates whether the lossless compression is enabled or not. (Default: `false`)

## [ImageSharp](https://github.com/SixLabors/ImageSharp)

Add the `Volo.Abp.Imaging.ImageSharp` NuGet package to your project and use the `AbpImagingImageSharpModule` module.

```csharp
[DependsOn(typeof(AbpImagingImageSharpModule))]
public class MyModule : AbpModule
{
    //...
}
```

### Configuration

#### ImageSharpCompressOptions

`ImageSharpCompressOptions` is used to configure the ImageSharp image compression system. It has the following properties:

* `DefaultQuality`: The default quality of the JPEG and WebP encoders. (Default: `75`)
* `JpegEncoder`: The JPEG encoder. (Default: `JpegEncoder` with `Quality` set to `DefaultQuality`)
* `PngEncoder`: The PNG encoder. (Default: `PngEncoder` with `IgnoreMetadata` set to `true` and `CompressionLevel` set to `PngCompressionLevel.BestCompression`)	
* `WebPEncoder`: The WebP encoder. (Default: `WebPEncoder` with `Quality` set to `DefaultQuality`)

## ASP.NET Core Integration

Add the `Volo.Abp.Imaging.AspNetCore` NuGet package to your project and use the `AbpImagingAspNetCoreModule` module.

> It needs a provider to work. You can use the [Magick.NET](#magicknet), [ImageSharp](#imagesharp) provider or implement your own provider. Provider does not exist, the input stream will be returned as it is.

```csharp
[DependsOn(typeof(AbpImagingAspNetCoreModule))]
public class MyModule : AbpModule
{
    //...
}
```

### CompressImageAttribute

The `CompressImageAttribute` is used to compress the image before requesting the action. IFormFile, IRemoteStreamContent, Stream and IEnumrable<byte> types are supported. It has the following properties:

* `Parameters`: The parameters that are used to configure the image compression system. (Optional. This applies to all parameters if left blank.)

Usage example:

```csharp

[CompressImage]
[HttpPost]
public async Task<IActionResult> Upload(IFormFile file)
{
    //...
}
```

### ResizeImageAttribute

The `ResizeImageAttribute` is used to resize the image before requesting the action. IFormFile, IRemoteStreamContent, Stream and IEnumrable<byte> types are supported. It has the following properties:

* `Parameters`: The parameters that are used to configure the image resize system. (Optional. This applies to all parameters if left blank.)
* `Width`: The width of the resized image.
* `Height`: The height of the resized image.
* `Mode`: The resize mode. (See the [ImageResizeMode](#imageresizemode) section for more information.)

Usage example:

```csharp

[ResizeImage(Width = 100, Height = 100, Mode = ImageResizeMode.Crop)]
[HttpPost]
public async Task<IActionResult> Upload(IFormFile file)
{
    //...
}
```