using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.TestApp.Application.Dto;

public class GetPersonPhonesFilter
{
    public PhoneType? Type { get; set; }
}
