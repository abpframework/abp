# Imaging
The ABP Framework provides an abstraction to work with image compress/resize. Having such an abstraction has some benefits;

* You can write library independent code. Therefore, you can change the underlying library with the minimum effort and code change.
* You can use the predefined image resizer/compressor defined in the ABP without worrying about the underlying library's internal details.

> The image resizer/compressor system is designed to be extensible. You can implement your own image resizer/compressor contributor and use it in your application.

## IImageResizer

You can inject `IImageResizer` and use it for image resize operations. Here is the available operations in the `IImageResizer` interface.

```csharp
public interface IImageResizer
{
    Task<ImageProcessResult<Stream>> ResizeAsync(Stream stream, ImageResizeArgs resizeArgs, [CanBeNull]string mimeType = null, CancellationToken cancellationToken = default);
    
    Task<ImageProcessResult<byte[]>> ResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, [CanBeNull] string mimeType = null, CancellationToken cancellationToken = default);
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

## IImageCompressor

You can inject `IImageCompressor` and use it for image compression operations. Here is the available operations in the `IImageCompressor` interface.

```csharp
public interface IImageCompressor
{
    Task<ImageProcessResult<Stream>> CompressAsync(
        Stream stream,
        [CanBeNull] string mimeType = null,
        CancellationToken cancellationToken = default);
    
    Task<ImageProcessResult<byte[]>> CompressAsync(
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

## ImageProcessResult

The `ImageProcessResult` is a generic class that is used to return the result of the image processing operations. It has the following properties:

* `Result`: The processed image stream or byte array.
* `IsSuccess`: Indicates whether the operation is successful or not. (If the operation is not successful, the `Result` property will be your input stream or byte array.)

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
    Task<ImageContributorResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
    
    Task<ImageContributorResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
}
```

## IImageCompressorContributor

The `IImageCompressorContributor` is an interface that is used to implement a custom image compressor. It has the following method:

```csharp
public interface IImageCompressorContributor
{
    Task<ImageContributorResult<Stream>> TryCompressAsync(Stream stream, string mimeType = null, CancellationToken cancellationToken = default);
    Task<ImageContributorResult<byte[]>> TryCompressAsync(byte[] bytes, string mimeType = null, CancellationToken cancellationToken = default);
}
```

## ImageContributorResult

The `ImageContributorResult` is a generic class that is used to return the result of the image processing operations. It has the following properties:

* `Result`: The processed image stream or byte array.
* `IsSuccess`: Indicates whether the operation is successful or not. (If the operation is not successful, the `Result` property will be your input stream or byte array.)
* `IsSupported`: Indicates whether the contributor supports the given image or not. (If the contributor does not support the given image, the `Result` property will be your input stream or byte array.)
* `Exception`: The exception that is thrown during the image processing operation. (If the operation is successful, this property will be null.)

## Configuration

### ImageResizeOptions

`ImageResizeOptions` is used to configure the image resize system. It has the following properties:

* `DefaultResizeMode`: The default resize mode. (Default: `ImageResizeMode.None`)

## Magick.NET

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

## ImageSharp

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

* `JpegQuality`: The JPEG quality. (Default: `60`)
* `PngCompressionLevel`: The PNG compression level. (Default: `PngCompressionLevel.Level9`)
* `PngIgnoreMetadata`: Indicates whether the PNG metadata is ignored or not. (Default: `true`)
* `WebpQuality`: The WebP quality. (Default: `60`)

## AspNetCore

Add the `Volo.Abp.Imaging.AspNetCore` NuGet package to your project and use the `AbpImagingAspNetCoreModule` module.

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