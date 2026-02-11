using System.Collections.Generic;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.TestApp.Application.Dto;

public class GetParamsInput : ExtensibleObject
{
    public List<GetParamsNameValue> NameValues { get; set; }

    public GetParamsNameValue NameValue { get; set; }
}

public class GetParamsNameValue : ExtensibleObject
{
    public string Name { get; set; }

    public string Value { get; set; }
}
