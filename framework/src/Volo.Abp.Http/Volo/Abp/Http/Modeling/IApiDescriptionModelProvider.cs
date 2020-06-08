namespace Volo.Abp.Http.Modeling
{
    public interface IApiDescriptionModelProvider
    {
        ApplicationApiDescriptionModel CreateApiModel(ApplicationApiDescriptionModelRequestDto input);
    }
}