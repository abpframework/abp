using System.Globalization;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.BlobStoring.Google;

public class GoogleBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    /// <summary>
    /// https://cloud.google.com/storage/docs/buckets#naming
    /// </summary>
    public string NormalizeContainerName(string containerName)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            // All letters in a Bucket name must be lowercase.
            containerName = containerName.ToLower();

            // Bucket names must contain 3-63 characters. Names containing dots can contain up to 222 characters, but each dot-separated component can be no longer than 63 characters.
            if(containerName.Contains("."))
            {
                if (containerName.Length > 222)
                {
                    containerName = containerName.Substring(0, 222);
                }
                
                var parts = containerName.Split('.');
                for (var i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Length > 63)
                    {
                        parts[i] = parts[i].Substring(0, 63);
                    }
                }

                containerName = string.Join(".", parts);
            }
            else if (containerName.Length > 63)
            {
                containerName = containerName.Substring(0, 63);
            }
            
            //Bucket names can only contain lowercase letters, numeric characters, dashes (-), underscores (_), and dots (.). Spaces are not allowed. Names containing dots require verification.
            containerName = Regex.Replace(containerName, "[^a-z0-9-_.]", string.Empty);

            //Be a syntactically valid DNS name (for example, bucket..example.com is not valid because it contains two dots in a row).
            containerName = Regex.Replace(containerName, "[.]{2,}", ".");
            
            //Bucket names cannot be represented as an IP address in dotted-decimal notation (for example, 192.168.5.4).
            containerName = Regex.Replace(containerName, "^(?:(?:^|\\.)(?:2(?:5[0-5]|[0-4]\\d)|1?\\d?\\d)){4}$", string.Empty);
            
            //Bucket names cannot begin with the "goog" prefix.
            containerName = Regex.Replace(containerName, "^goog", string.Empty);
            
            //Bucket names cannot contain "google" or close misspellings, such as "g00gle".
            containerName = Regex.Replace(containerName, "google", string.Empty);
            
            //Bucket names must start and end with a number or letter.
            containerName = RemoveInvalidStartEndCharacters(containerName);
            
            // Bucket names must be from 3 through 63 characters long. Names containing dots can contain up to 222 characters.
            if (containerName.Length < 3)
            {
                var length = containerName.Length;
                for (var i = 0; i < 3 - length; i++)
                {
                    containerName += "0";
                }
            }

            return containerName;
        }
    }
    
    protected virtual string RemoveInvalidStartEndCharacters(string containerName)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            return containerName;
        }
        
        if (!char.IsLetterOrDigit(containerName[0]))
        {
            containerName = containerName.Substring(1);
            return RemoveInvalidStartEndCharacters(containerName);
        }

        if (!char.IsLetterOrDigit(containerName[containerName.Length - 1]))
        {
            containerName = containerName.Substring(0, containerName.Length - 1);
            return RemoveInvalidStartEndCharacters(containerName);
        }

        return containerName;
    }

    /// <summary>
    /// https://cloud.google.com/storage/docs/objects#naming
    /// </summary>
    public string NormalizeBlobName(string blobName)
    {
        //Object names can contain any sequence of valid Unicode characters, of length 1-1024 bytes when UTF-8 encoded
        if (blobName.Length > 1024)
        {
            blobName = blobName.Substring(0, 1024);
        }
        
        //Object names cannot contain Carriage Return or Line Feed characters.
        blobName = Regex.Replace(blobName, "[\r\n]", string.Empty);
        
        //Object names cannot start with .well-known/acme-challenge/.
        blobName = Regex.Replace(blobName, "^\\.well-known/acme-challenge/", string.Empty);
        
        //Objects cannot be named . or ...
        blobName = Regex.Replace(blobName, "^\\.\\.?$", string.Empty);

        return blobName;
    }
}