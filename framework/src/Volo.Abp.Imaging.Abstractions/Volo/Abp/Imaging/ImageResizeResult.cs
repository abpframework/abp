namespace Volo.Abp.Imaging;

public class ImageResizeResult<T> : ImageProcessResult<T>
{
    public ImageResizeResult(T result, ProcessState state) : base(result, state)
    {
    }
}