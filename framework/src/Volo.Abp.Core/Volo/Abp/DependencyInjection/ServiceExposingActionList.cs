using System;
using System.Collections.Generic;

namespace Volo.Abp.DependencyInjection;

public class ServiceExposingActionList : List<Action<IOnServiceExposingContext>>
{

}
