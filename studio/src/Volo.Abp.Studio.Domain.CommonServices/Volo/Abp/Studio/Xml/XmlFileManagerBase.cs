using System;
using System.IO;
using System.IO.Abstractions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Volo.Abp.Threading;

namespace Volo.Abp.Studio.Xml
{
    public abstract class XmlFileManagerBase
    {
        public IFileSystem FileSystem { get; set; }

        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        protected async Task<XmlDocument> GetXmlDocumentAsync(string filePath)
        {
            try
            {
                var doc = new XmlDocument() { PreserveWhitespace = true };
                doc.Load(GenerateStreamFromString(await FileSystem.File.ReadAllTextAsync(filePath)));
                return doc;
            }
            catch (Exception ex)
            {
                throw new AbpException($"Error while reading {filePath} as XML document.", innerException: ex);
            }
        }

        protected async Task SaveXmlDocumentAsync(string filePath, XmlDocument rootNode)
        {
            await SaveFileContentAsync(filePath, XDocument.Parse(rootNode.OuterXml).ToString());
        }

        protected async Task SaveFileContentAsync(string filePath, string content)
        {
            await FileSystem.File.WriteAllTextAsync(filePath, content, CancellationTokenProvider.Token);
        }

        private MemoryStream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
