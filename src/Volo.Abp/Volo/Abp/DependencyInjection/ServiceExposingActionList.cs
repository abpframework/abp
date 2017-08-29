using System;
using System.Collections.Generic;

namespace Volo.DependencyInjection
{
    public class ServiceExposingActionList : List<Action<IOnServiceExposingContext>>
    {

    }
}