using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Aspects;

public static class AbpCrossCuttingConcerns
{
    //TODO: Move these constants to their own assemblies!

    public const string Auditing = "AbpAuditing";
    public const string UnitOfWork = "AbpUnitOfWork";
    public const string FeatureChecking = "AbpFeatureChecking";
    public const string GlobalFeatureChecking = "AbpGlobalFeatureChecking";

    public static void AddApplied(object obj, params string[] concerns)
    {
        if (concerns.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(concerns), $"{nameof(concerns)} should be provided!");
        }

        (obj as IAvoidDuplicateCrossCuttingConcerns)?.AppliedCrossCuttingConcerns.AddRange(concerns);
    }

    public static void RemoveApplied(object obj, params string[] concerns)
    {
        if (concerns.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(concerns), $"{nameof(concerns)} should be provided!");
        }

        var crossCuttingEnabledObj = obj as IAvoidDuplicateCrossCuttingConcerns;
        if (crossCuttingEnabledObj == null)
        {
            return;
        }

        foreach (var concern in concerns)
        {
            crossCuttingEnabledObj.AppliedCrossCuttingConcerns.RemoveAll(c => c == concern);
        }
    }

    public static bool IsApplied([NotNull] object obj, [NotNull] string concern)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (concern == null)
        {
            throw new ArgumentNullException(nameof(concern));
        }

        return (obj as IAvoidDuplicateCrossCuttingConcerns)?.AppliedCrossCuttingConcerns.Contains(concern) ?? false;
    }

    public static IDisposable Applying(object obj, params string[] concerns)
    {
        AddApplied(obj, concerns);
        return new DisposeAction(() =>
        {
            RemoveApplied(obj, concerns);
        });
    }

    public static string[] GetApplieds(object obj)
    {
        var crossCuttingEnabledObj = obj as IAvoidDuplicateCrossCuttingConcerns;
        if (crossCuttingEnabledObj == null)
        {
            return new string[0];
        }

        return crossCuttingEnabledObj.AppliedCrossCuttingConcerns.ToArray();
    }
}
