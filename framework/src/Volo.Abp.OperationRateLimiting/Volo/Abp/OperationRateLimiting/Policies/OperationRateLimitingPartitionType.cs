namespace Volo.Abp.OperationRateLimiting;

public enum OperationRateLimitingPartitionType
{
    Parameter,
    CurrentUser,
    CurrentTenant,
    ClientIp,
    Email,
    PhoneNumber,
    Custom
}
