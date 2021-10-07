using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Studio.Helpers;
using Volo.Abp.Studio.Package;
using Volo.Abp.Studio.Xml;

namespace Volo.Abp.Studio.Packages.Modifying
{
    public class CsprojFileManager: XmlFileManagerBase, ICsprojFileManager, ITransientDependency
    {
        public async Task AddProjectReferenceAsync(string filePath, string projectToReference)
        {
            var document = await GetXmlDocumentAsync(filePath);

            /*if (document.SelectNodes($"/Project/ItemGroup/ProjectReference[ends-with(@Include, '{Path.GetFileName(projectToReference)}')]").Count > 0)
            {
                return;
            }*/

            var packageReferenceToSameProject =
                document.SelectNodes(
                    $"/Project/ItemGroup/PackageReference[starts-with(@Include, '{Path.GetFileName(projectToReference).RemovePostFix(".csproj")}')]"
                    );

            if (packageReferenceToSameProject.Count > 0)
            {
                packageReferenceToSameProject[0].ParentNode.RemoveChild(packageReferenceToSameProject[0]);
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

        public async Task ConvertPackageReferenceToProjectReferenceAsync(string filePath, string projectToReference)
        {
            var document = await GetXmlDocumentAsync(filePath);

            var packageName = Path.GetFileName(projectToReference).RemovePostFix(".csproj");

            var matchedNodes = document.SelectNodes($"/Project/ItemGroup/PackageReference[starts-with(@Include, '{packageName}')]");

            if (matchedNodes.Count == 0)
            {
                return;
            }

            XmlNode targetNode = null;

            foreach (XmlNode matchedNode in matchedNodes)
            {
                if (matchedNode.Attributes["Include"].Value == packageName)
                {
                    targetNode = matchedNode;
                    break;
                }
            }

            if (targetNode == null)
            {
                return;
            }

            var targetNodeParent = targetNode.ParentNode;

            targetNodeParent.RemoveChild(targetNode);

            var relativePath = PathHelper.GetRelativePath(filePath, projectToReference);

            var newNode = document.CreateElement("ProjectReference");

            var includeAttr = document.CreateAttribute("Include");
            includeAttr.Value = relativePath;
            newNode.Attributes.Append(includeAttr);

            targetNodeParent.AppendChild(newNode);

            await SaveXmlDocumentAsync(filePath, document);
        }

        public async Task<string> GetTargetFrameworkAsync(string filePath)
        {
            var document = await GetXmlDocumentAsync(filePath);

            var nodes = document["Project"]?["PropertyGroup"]?.SelectNodes("TargetFramework");

            if (nodes == null || nodes.Count == 0)
            {
                return null;
            }

            return nodes[0].InnerText.Trim();
        }

        public async Task<List<PackageDependency>> GetDependencyListAsync(string filePath)
        {
            var result = new List<PackageDependency>();

            var document = await GetXmlDocumentAsync(filePath);

            var packageReferenceNodes = document.SelectNodes($"/Project/ItemGroup/PackageReference");
            var projectReferenceNodes = document.SelectNodes($"/Project/ItemGroup/ProjectReference");

            foreach (XmlNode packageReferenceNode in packageReferenceNodes)
            {
                result.Add(
                        new PackageDependency(
                                packageReferenceNode.Attributes["Include"].Value,
                                packageReferenceNode.Attributes["Version"].Value
                            )
                    );
            }

            foreach (XmlNode projectReferenceNode in projectReferenceNodes)
            {
                result.Add(
                        new PackageDependency(
                            Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filePath), projectReferenceNode.Attributes["Include"].Value))
                            )
                    );
            }

            return result;
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
