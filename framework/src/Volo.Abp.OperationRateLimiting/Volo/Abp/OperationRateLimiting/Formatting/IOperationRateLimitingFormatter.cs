using System;

namespace Volo.Abp.OperationRateLimiting;

public interface IOperationRateLimitingFormatter
{
    string Format(TimeSpan duration);
}
