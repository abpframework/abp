namespace Volo.Abp.ApiVersioning
{
    public interface IRequestedApiVersion
    {
        string Current { get; }
    }
}
