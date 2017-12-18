namespace Volo.Abp
{
    public interface IAbpApplicationWithInternalServiceProvider : IAbpApplication
    {
        void Initialize();
    }
}