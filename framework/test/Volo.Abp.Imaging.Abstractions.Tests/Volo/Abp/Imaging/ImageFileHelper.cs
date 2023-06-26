using System;
using System.IO;

namespace Volo.Abp.Imaging;

public static class ImageFileHelper
{
    public static Stream GetJpgTestFileStream()
    {
        return GetTestFileStream("abp.jpg");
    }
    
    public static Stream GetPngTestFileStream()
    {
        return GetTestFileStream("abp.png");
    }
    
    public static Stream GetWebpTestFileStream()
    {
        return GetTestFileStream("abp.webp");
    }
    
    private static Stream GetTestFileStream(string fileName)
    {
        var assembly = typeof(ImageFileHelper).Assembly;
        var resourceStream = assembly.GetManifestResourceStream("Volo.Abp.Imaging.Files." + fileName);
        if (resourceStream == null)
        {
            throw new Exception($"File {fileName} does not exists!");
        }
        
        return resourceStream;
    }
}