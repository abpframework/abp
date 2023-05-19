using System;
using System.Threading.Tasks;
using Volo.Abp.Features;

namespace Volo.Abp.AspNetCore.Mvc.Features;

public class FakeFeatureChecker : FeatureCheckerBase
{
    public override Task<string> GetOrNullAsync(string name)
    {
        return Task.FromResult(GetOrNull(name));
    }

    private static string GetOrNull(string name)
    {
        switch (name)
        {
            case "AllowedFeature":
                return true.ToString();
            case "NotAllowedFeature":
                return null; //or false, doesn't matter
        }

        throw new ApplicationException($"Unknown feature: '{name}'");
    }
}
