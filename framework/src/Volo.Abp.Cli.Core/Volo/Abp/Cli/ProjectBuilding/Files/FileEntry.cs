using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Volo.Abp.Cli.ProjectBuilding.Files;

public class FileEntry
{
    private static string[] BinaryFileExtensions = {
            ".exe", ".dll", ".bin",
            ".suo", ".obj", ".pdb",
            ".png", "jpg", "jpeg", ".ico"
            ,".woff", ".woff2", ".eot", ".svg", ".ttf"
        };

    public string Name { get; private set; }

    public bool IsDirectory { get; }

    public Encoding Encoding { get; }

    public byte[] Bytes { get; private set; }

    public string Content { get; private set; }

    public bool IsBinaryFile { get; private set; }

    public FileEntry(string name, byte[] bytes, bool isDirectory)
    {
        Name = name;
        Bytes = bytes;
        IsDirectory = isDirectory;

        Encoding = CalculateEncoding();
        Content = Encoding.GetString(bytes);
        IsBinaryFile = CalculateIsBinaryFile();
    }

    public void SetContent(string fileContent)
    {
        Content = fileContent;
        Bytes = Encoding.GetBytes(fileContent);
    }

    public string[] GetLines()
    {
        return Content.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
    }

    public void SetLines(IEnumerable<string> lines)
    {
        SetContent(lines.JoinAsString(Environment.NewLine));
    }

    public void SetName(string fileName)
    {
        Name = fileName;
        IsBinaryFile = CalculateIsBinaryFile();
    }

    public void NormalizeLineEndings()
    {
        if (Content.IsNullOrEmpty())
        {
            return;
        }

        SetContent(Content.NormalizeLineEndings());
    }

    public override string ToString()
    {
        var str = new StringBuilder(Name);

        if (IsDirectory)
        {
            str.Append(" [DIR]");
        }

        str.Append($" [{Bytes.Length}]");

        return str.ToString();
    }

    private Encoding CalculateEncoding()
    {
        if (Bytes.IsNullOrEmpty())
        {
            return Encoding.ASCII;
        }

        if (Bytes[0] == 0x2b && Bytes[1] == 0x2f && Bytes[2] == 0x76)
        {
            return Encoding.UTF7;
        }

        if (Bytes[0] == 0xef && Bytes[1] == 0xbb && Bytes[2] == 0xbf)
        {
            return Encoding.UTF8;
        }

        if (Bytes[0] == 0xff && Bytes[1] == 0xfe)
        {
            return Encoding.Unicode; //UTF-16LE
        }

        if (Bytes[0] == 0xfe && Bytes[1] == 0xff)
        {
            return Encoding.BigEndianUnicode; //UTF-16BE
        }

        if (Bytes[0] == 0 && Bytes[1] == 0 && Bytes[2] == 0xfe && Bytes[3] == 0xff)
        {
            return Encoding.UTF32;
        }

        return Encoding.UTF8;
    }

    private bool CalculateIsBinaryFile()
    {
        return BinaryFileExtensions.Any(ext => Name.EndsWith(ext));
    }
}
