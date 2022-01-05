using System.Collections.Generic;

namespace Volo.Abp.TestApp.Application.Dto;

public class GetParamsInput
{
    public List<GetParamsNameValue> NameValues { get; set; }

    public GetParamsNameValue NameValue { get; set; }
}

public class GetParamsNameValue
{
    public string Name { get; set; }

    public string Value { get; set; }
}
