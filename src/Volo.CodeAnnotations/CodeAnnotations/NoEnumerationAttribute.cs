using System;

namespace JetBrains.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class NoEnumerationAttribute : Attribute
    {
    }
}