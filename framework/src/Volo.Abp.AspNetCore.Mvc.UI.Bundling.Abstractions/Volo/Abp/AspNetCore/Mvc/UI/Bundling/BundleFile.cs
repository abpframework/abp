using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleFile : IEquatable<BundleFile>, IComparable<BundleFile>
{
    public string FileName { get; }

    public bool IsExternalFile { get; }

    public BundleFile(string fileName)
    {
        FileName = fileName;
        IsExternalFile = fileName.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                fileName.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    public BundleFile(string fileName, bool isExternalFile)
    {
        FileName = fileName;
        IsExternalFile = isExternalFile;
    }

    /// <summary>
    /// This method is used to compatible with old code.
    /// </summary>
    public static implicit operator BundleFile(string fileName)
    {
        return new BundleFile(fileName);
    }

    public bool Equals(BundleFile? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return FileName == other.FileName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((BundleFile)obj);
    }

    public override int GetHashCode()
    {
        return FileName.GetHashCode();
    }

    public int CompareTo(BundleFile? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (ReferenceEquals(null, other))
        {
            return 1;
        }

        return string.Compare(FileName, other.FileName, StringComparison.Ordinal);
    }
}
