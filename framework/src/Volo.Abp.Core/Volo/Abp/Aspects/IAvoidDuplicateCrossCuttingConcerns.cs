using System.Collections.Generic;

namespace Volo.Abp.Aspects;

public interface IAvoidDuplicateCrossCuttingConcerns
{
    List<string> AppliedCrossCuttingConcerns { get; }
}
