using System.Collections.Generic;

namespace Volo.Abp.TestBase.Logging
{
    public interface ICanLogOnObject
    {
        List<string> Logs { get; }
    }
}