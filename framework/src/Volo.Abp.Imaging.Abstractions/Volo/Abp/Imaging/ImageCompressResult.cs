namespace Volo.Abp.Imaging;

public class ImageCompressResult<T> : ImageProcessResult<T>
{
    public ImageCompressResult(T result, ImageProcessState state) : base(result, state)
    {
    }
}