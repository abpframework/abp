using JetBrains.Annotations;

namespace Volo.Abp.Studio.Analyzing;

public class AnalyzingOptions
{
    public bool Force { get; set; } = false;

    [CanBeNull]
    public string AnalyzeConfigurationFile { get; set; }

    // Combines two options
    // The second option has more priority
    public static AnalyzingOptions Combine([CanBeNull] AnalyzingOptions first, [CanBeNull] AnalyzingOptions second)
    {
        if (second == null && first == null)
        {
            return new AnalyzingOptions();
        }

        if (second == null)
        {
            return first;
        }

        if (first == null)
        {
            return second;
        }

        return new AnalyzingOptions
        {
            AnalyzeConfigurationFile = second.AnalyzeConfigurationFile ?? first.AnalyzeConfigurationFile
        };
    }
}
