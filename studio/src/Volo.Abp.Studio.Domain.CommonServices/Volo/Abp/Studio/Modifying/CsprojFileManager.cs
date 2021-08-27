using System;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.Helpers;
using Volo.Abp.Studio.Xml;

namespace Volo.Abp.Studio.Packages.Modifying
{
    public class CsprojFileManager: XmlFileManagerBase, ICsprojFileManager, ITransientDependency
    {
        public async Task AddProjectReferenceAsync(string filePath, string projectToReference)
        {
            var document = await GetXmlDocumentAsync(filePath);

            if (document.SelectNodes($"/Project/ItemGroup/ProjectReference[starts-with(@Include, '{projectToReference}')]").Count > 0)
            {
                // Project reference is already added.
                return;
            }

            var relativePath = PathHelper.GetRelativePath(filePath, projectToReference);

            var itemGroupNode = GetOrCreateItemGroupNode(document);

            var newNode = document.CreateElement("ProjectReference");

            var includeAttr = document.CreateAttribute("Include");
            includeAttr.Value = relativePath;
            newNode.Attributes.Append(includeAttr);

            itemGroupNode.AppendChild(newNode);

            await SaveXmlDocumentAsync(filePath, document);
        }

        public async Task AddPackageReferenceAsync(string filePath, string packageName, string version)
        {
            var document = await GetXmlDocumentAsync(filePath);

            if (document.SelectNodes($"/Project/ItemGroup/PackageReference[starts-with(@Include, '{packageName}')]").Count > 0)
            {
                // Package reference is already added.
                return;
            }

            var itemGroupNode = GetOrCreateItemGroupNode(document);

            var newNode = document.CreateElement("PackageReference");

            var includeAttr = document.CreateAttribute("Include");
            includeAttr.Value = packageName;
            newNode.Attributes.Append(includeAttr);

            var versionAttr = document.CreateAttribute("Version");
            versionAttr.Value = version;
            newNode.Attributes.Append(versionAttr);

            itemGroupNode.AppendChild(newNode);
            itemGroupNode.AppendChild(document.CreateWhitespace(Environment.NewLine + "  "));

            await SaveXmlDocumentAsync(filePath, document);
        }

        private XmlNode GetOrCreateItemGroupNode(XmlDocument document)
        {
            var nodes = document["Project"].SelectNodes("ItemGroup");

            if (nodes == null || nodes.Count < 1)
            {
                var newNode = document.CreateElement("ItemGroup");
                document["Project"].AppendChild(newNode);
                return newNode;
            }

            foreach (XmlNode node in nodes)
            {
                if (node.SelectNodes("ProjectReference").Count > 0)
                {
                    return node;
                }
            }

            foreach (XmlNode node in nodes)
            {
                if (node.SelectNodes("PackageReference").Count > 0)
                {
                    return node;
                }
            }

            return nodes[0];
        }
    }
}
