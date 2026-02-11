using System;
using System.IO;

namespace Volo.Abp.AspNetCore.RazorViews;

public class HelperResult
{
    public HelperResult(Action<TextWriter> action)
    {
        WriteAction = action;
    }

    public Action<TextWriter> WriteAction { get; }

    public void WriteTo(TextWriter writer)
    {
        WriteAction(writer);
    }
}
