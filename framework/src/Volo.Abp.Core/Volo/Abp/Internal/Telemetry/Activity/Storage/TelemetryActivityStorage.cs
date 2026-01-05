using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal.Telemetry.Activity.Contracts;
using Volo.Abp.Internal.Telemetry.Constants;
using Volo.Abp.Internal.Telemetry.Helpers;

namespace Volo.Abp.Internal.Telemetry.Activity.Storage;

public class TelemetryActivityStorage : ITelemetryActivityStorage, ISingletonDependency
{
    private TelemetryActivityStorageState State { get; }
    private readonly static JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, 
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public TelemetryActivityStorage()
    {
        CreateDirectoryIfNotExist();
        
        DeleteExistingOldInformation();

        State = LoadState();
    }

    public void SaveActivity(ActivityEvent activityEvent)
    {
        State.Activities.Add(activityEvent);

        var activityName = activityEvent.Get<string>(ActivityPropertyNames.ActivityName);
        
        if (activityName == ActivityNameConsts.AbpStudioClose || activityName == ActivityNameConsts.AbpStudioCloseWithoutLogin)
        {
            State.SessionId = null;
        }

        if (activityEvent.HasDeviceInfo())
        {
            State.LastDeviceInfoAddTime = DateTimeOffset.UtcNow;
        }

        if (activityEvent.HasSolutionInfo())
        {
            var solutionId = activityEvent.Get<Guid?>(ActivityPropertyNames.SolutionId);
            if (solutionId.HasValue && solutionId.Value != Guid.Empty)
            {
                State.Solutions[solutionId.Value] = DateTimeOffset.UtcNow;
            }
        }

        if (activityEvent.HasProjectInfo())
        {
            var projectId = activityEvent.Get<Guid?>(ActivityPropertyNames.ProjectId);
            if (projectId.HasValue && projectId.Value != Guid.Empty)
            {
                State.Projects[projectId.Value] = DateTimeOffset.UtcNow;
            }
        }

        SaveState();
    }

    public List<ActivityEvent> GetActivities()
    {
        return new List<ActivityEvent>(State.Activities);
    }

    public Guid InitializeOrGetSession()
    {
        if (State.SessionId.HasValue)
        {
            return State.SessionId.Value;
        }

        State.SessionId = Guid.NewGuid();
        SaveState();

        return State.SessionId.Value;
    }

    public void DeleteActivities(ActivityEvent[] activities)
    {
        var activityIds = new HashSet<Guid>(activities.Select(x => x.Get<Guid>(ActivityPropertyNames.Id)));
        
        State.Activities.RemoveAll(x => activityIds.Contains(x.Get<Guid>(ActivityPropertyNames.Id)));
        
        SaveState();
    }
    
    public void MarkActivitiesAsFailed(ActivityEvent[] activities)
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var activity in activities)
        {
            var activityId = activity.Get<Guid>(ActivityPropertyNames.Id);
            
            if (State.FailedActivities.TryGetValue(activityId, out var failedActivityInfo))
            {
                failedActivityInfo.RetryCount++;
                failedActivityInfo.LastFailTime = now;

                if (!failedActivityInfo.IsExpired())
                {
                    continue;
                }

                State.Activities.RemoveAll(x=> x.Get<Guid>(ActivityPropertyNames.Id) == activityId);
                State.FailedActivities.Remove(activityId);
            }
            else
            {
                State.FailedActivities[activityId] = new FailedActivityInfo
                {
                    FirstFailTime = now, 
                    LastFailTime = now,
                    RetryCount = 1
                };
            }
        }

        SaveState();
    }
    
    public bool ShouldAddDeviceInfo()
    {
        return State.LastDeviceInfoAddTime is null ||
               DateTimeOffset.UtcNow - State.LastDeviceInfoAddTime > TelemetryPeriod.InformationSendPeriod;
    }
    
    public bool ShouldAddSolutionInformation(Guid solutionId)
    {
        return !State.Solutions.TryGetValue(solutionId, out var lastSend) ||
               DateTimeOffset.UtcNow - lastSend > TelemetryPeriod.InformationSendPeriod;
    }

    public bool ShouldAddProjectInfo(Guid projectId)
    {
        return !State.Projects.TryGetValue(projectId, out var lastSend) ||
               DateTimeOffset.UtcNow - lastSend > TelemetryPeriod.InformationSendPeriod;
    }

    public bool ShouldSendActivities()
    {
        return State.ActivitySendTime is null ||
               DateTimeOffset.UtcNow - State.ActivitySendTime > TelemetryPeriod.ActivitySendPeriod;
    }
    
    private void SaveState()
    {
        try
        {
            var json = JsonSerializer.Serialize(State, JsonSerializerOptions);
            var encryptedJson = Cryptography.Encrypt(json);
            File.WriteAllText(TelemetryPaths.ActivityStorage, encryptedJson, Encoding.UTF8);
        }
        catch
        {
            // Ignored 
        }
    }

    private static void DeleteExistingOldInformation()
    {
        try
        {
            var file = new FileInfo(TelemetryPaths.ActivityStorage);
            if (file.Exists && file.CreationTime < new DateTime(2025, 12, 01))
            {
                file.Delete();
            }
        }
        catch
        {
            // Ignored 
        }
    }

    private static TelemetryActivityStorageState LoadState()
    {
        try
        {
            if (!File.Exists(TelemetryPaths.ActivityStorage))
            {
                return new TelemetryActivityStorageState();
            }

            var fileContent = MutexExecutor.ReadFileSafely(TelemetryPaths.ActivityStorage);

            if (fileContent.IsNullOrEmpty())
            {
                return new TelemetryActivityStorageState();
            }

            var json = Cryptography.Decrypt(fileContent);

            var state = JsonSerializer.Deserialize<TelemetryActivityStorageState>(json, JsonSerializerOptions)!;
            state.Activities = state.Activities.Where(x => x != null).ToList();
            return state;
        }
        catch
        {
            return new TelemetryActivityStorageState();
        }
    }

    private static void CreateDirectoryIfNotExist()
    {
        try
        {
            var storageDirectory = Path.GetDirectoryName(TelemetryPaths.ActivityStorage)!;

            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
        }
        catch
        {
            // Ignored 
        }
    }
}