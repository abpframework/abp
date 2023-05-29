namespace Volo.Abp.Imaging;

public class ImageResizeResult<T> : ImageProcessResult<T>
{
    public ImageResizeResult(T result, ImageProcessState state) : base(result, state)
    {
    }
}