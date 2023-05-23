namespace Volo.Abp.Imaging;

public class ImageCompressResult<T> : ImageProcessResult<T>
{
    public ImageCompressResult(T result, ProcessState state) : base(result, state)
    {
    }
}