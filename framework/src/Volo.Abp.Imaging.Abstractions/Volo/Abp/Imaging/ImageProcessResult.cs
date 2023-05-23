namespace Volo.Abp.Imaging;

public abstract class ImageProcessResult<T>
{
    public T Result { get; }
    public ProcessState State { get; }

    protected ImageProcessResult(T result, ProcessState state)
    {
        Result = result;
        State = state;
    }
}