namespace Volo.Abp.Imaging;

public abstract class ImageProcessResult<T>
{
    public T Result { get; }
    public ImageProcessState State { get; }

    protected ImageProcessResult(T result, ImageProcessState state)
    {
        Result = result;
        State = state;
    }
}