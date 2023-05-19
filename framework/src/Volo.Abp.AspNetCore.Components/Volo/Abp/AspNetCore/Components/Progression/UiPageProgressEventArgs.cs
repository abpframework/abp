using System;

namespace Volo.Abp.AspNetCore.Components.Progression;

public class UiPageProgressEventArgs : EventArgs
{
    public UiPageProgressEventArgs(int? percentage, UiPageProgressOptions options)
    {
        Percentage = percentage;
        Options = options;
    }

    public int? Percentage { get; }

    public UiPageProgressOptions Options { get; }
}
