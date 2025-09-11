using System;
using System.Collections.Generic;

namespace Volo.Abp.Internal.Telemetry.Activity.Storage;

internal class TelemetryActivityStorageState
{
    public DateTimeOffset? ActivitySendTime { get; set; }
    public DateTimeOffset? LastDeviceInfoAddTime { get; set; }
    public Guid? SessionId { get; set; }
    public List<ActivityEvent> Activities { get; set; } = new();
    public Dictionary<Guid,DateTimeOffset> Solutions { get; set; } = new();
    public Dictionary<Guid, DateTimeOffset> Projects { get; set; } = new();
    public Dictionary<Guid, FailedActivityInfo> FailedActivities { get; set; } = new();

}