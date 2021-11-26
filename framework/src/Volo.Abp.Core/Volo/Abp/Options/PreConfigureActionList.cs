using System;
using System.Collections.Generic;

namespace Volo.Abp.Options;

public class PreConfigureActionList<TOptions> : List<Action<TOptions>>
{
    public void Configure(TOptions options)
    {
        foreach (var action in this)
        {
            action(options);
        }
    }
}
