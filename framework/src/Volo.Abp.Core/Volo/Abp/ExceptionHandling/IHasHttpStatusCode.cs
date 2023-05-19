namespace Volo.Abp.ExceptionHandling;

public interface IHasHttpStatusCode
{
    int HttpStatusCode { get; }
}
