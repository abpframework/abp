using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Volo.Abp.Cli.Commands
{
    public class GenerateProxyCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<GenerateProxyCommand> Logger { get; set; }

        protected TemplateProjectBuilder TemplateProjectBuilder { get; }

        public GenerateProxyCommand(TemplateProjectBuilder templateProjectBuilder)
        {
            TemplateProjectBuilder = templateProjectBuilder;

            Logger = NullLogger<GenerateProxyCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var apiUrl = commandLineArgs.Options.GetOrNull(Options.ApiUrl.Short, Options.ApiUrl.Long);
            var uiFramework = GetUiFramework(commandLineArgs); 

            //WebClient client = new WebClient();
            //string json = client.DownloadString(apiUrl);
            var sr = File.OpenText("api-definition.json");
            var json = sr.ReadToEnd();

            Logger.LogInformation("Downloading api definition...");
            Logger.LogInformation("Api Url: " + apiUrl);

            var data = JObject.Parse(json);  

            Logger.LogInformation("Modules are combining"); 
            var moduleList = GetCombinedModules(data);
                 
            Logger.LogInformation("Modules and types are creating"); 

            var serviceIndexList = new List<string>();
            var modelIndexList = new List<string>();
                 
            foreach (var module in moduleList)
            {
                var moduleValue = JObject.Parse(module.Value); 

                var rootPath = module.Key;
                var controllerName = "";
                var controllerServiceName = "";

                Logger.LogInformation($"{rootPath} directory is creating");

                Directory.CreateDirectory($"src/app/{rootPath}/shared/models");
                Directory.CreateDirectory($"src/app/{rootPath}/shared/services");

                var serviceFileText = new StringBuilder();

                serviceFileText.AppendLine("[firstTypeList]");
                serviceFileText.AppendLine("import { Injectable } from '@angular/core';");
                serviceFileText.AppendLine("import { Observable } from 'rxjs';");
                serviceFileText.AppendLine("[secondTypeList]");
                serviceFileText.AppendLine("");
                serviceFileText.AppendLine("@Injectable()");
                serviceFileText.AppendLine("export class [controllerName]Service {");
                serviceFileText.AppendLine("  constructor(private restService: RestService) {}");
                serviceFileText.AppendLine("");

                var firstTypeList = new List<string>();
                var secondTypeList = new List<string>();

                foreach (var controller in moduleValue.Root.ToList().Select(item => item.First))
                {
                    controllerName = (string)controller["controllerName"];
                    controllerServiceName = controllerName.PascalToKebabCase() + ".service.ts"; 

                    foreach (var actionItem in controller["actions"])
                    {
                        var action = actionItem.First;
                        var actionName = (string)action["uniqueName"];

                        actionName = (char.ToLower(actionName[0]) + actionName.Substring(1)).Replace("Async", "").Replace("Controller", "");

                        var returnValueType = (string)action["returnValue"]["type"];

                        var parameters = action["parameters"];
                        var parametersText = new StringBuilder();
                        var parametersIndex = 0;
                        var bodyExtra = "";
                        var modelBindingExtra = "";
                        var modelBindingExtraList = new List<string>();

                        foreach (var parameter in parameters)
                        {
                            parametersIndex++;

                            if (parametersIndex > 1)
                            {
                                parametersText.Append(", ");
                            }

                            var bindingSourceId = (string)parameter["bindingSourceId"];
                            bindingSourceId = char.ToLower(bindingSourceId[0]) + bindingSourceId.Substring(1);

                            if (bindingSourceId == "body")
                            {
                                bodyExtra = ", body";
                                var typeArray = ((string)parameter["type"]).Split(".");
                                var type = typeArray[typeArray.Length - 1];

                                parametersText.Append(bindingSourceId + ": " + type);
                                secondTypeList.Add(type);
                            }
                            else if (bindingSourceId == "path")
                            {
                                parametersText.Append((string)parameter["name"] + ": " + (string)parameter["typeSimple"]);
                            }
                            else if (bindingSourceId == "modelBinding")
                            {
                                var typeSimple = "";
                                var type = "";

                                var parameterNameOnMethod = (string)parameter["nameOnMethod"];

                                var parametersOnMethod = action["parametersOnMethod"];
                                foreach (var parameterOnMethod in parametersOnMethod)
                                {
                                    var parametersOnMethodName = (string)parameterOnMethod["name"];
                                    if (parametersOnMethodName == parameterNameOnMethod)
                                    {
                                        typeSimple = (string)parameterOnMethod["typeSimple"];

                                        var typeArray = ((string)parameterOnMethod["type"]).Split(".");
                                        type = typeArray[typeArray.Length - 1];
                                    }
                                }

                                if (typeSimple == "string" || typeSimple == "boolean" || typeSimple == "number")
                                {
                                    parametersText.Append((string)parameter["name"] + ": " + (string)parameter["typeSimple"]);
                                    modelBindingExtraList.Add((string)parameter["name"]);
                                }
                                else
                                {
                                    parametersText.Append($"params = {{}} as {type}");
                                    modelBindingExtra = ", params";
                                    secondTypeList.Add(type);
                                    break;
                                }
                            }
                        }

                        if (returnValueType != null)
                        {
                            if (returnValueType.IndexOf('<') > -1)
                            {
                                var firstTypeArray = returnValueType.Split("<")[0].Split(".");
                                var firstType = firstTypeArray[firstTypeArray.Length - 1];

                                var secondTypeArray = returnValueType.Split("<")[1].Split(".");
                                var secondType = secondTypeArray[secondTypeArray.Length - 1].TrimEnd('>');

                                serviceFileText.AppendLine(
                                    $" {actionName}({parametersText}): Observable<{firstType}<{secondType}>> {{");

                                firstTypeList.Add(firstType);
                                secondTypeList.Add(secondType);
                            }
                            else
                            {
                                var typeArray = returnValueType.Split(".");
                                var type = typeArray[typeArray.Length - 1].TrimEnd('>');

                                if (type == "Void")
                                {
                                    type = "void";
                                }

                                serviceFileText.AppendLine(
                                    $" {actionName}({parametersText}): Observable<{type}> {{");

                                secondTypeList.Add(type);
                            }

                            var modelIndex = CreateType(data, returnValueType, rootPath);

                            if (!IsNullOrWhiteSpace(modelIndex))
                            {
                                modelIndexList.Add(modelIndex);
                            }
                        }

                        if (modelBindingExtraList != null && modelBindingExtraList.Count > 0)
                        {
                            modelBindingExtra = ", params: { " + Join(", ", modelBindingExtraList.ToArray()) + " }";
                        }

                        var url = ((string)action["url"]).Replace("/{", "/${");
                        var httpMethod = (string)action["httpMethod"];
                        serviceFileText.AppendLine(
                            $"   return this.restService.request({{ url: '/{url}', method: '{httpMethod}'{bodyExtra}{modelBindingExtra} }});");

                        serviceFileText.AppendLine(" }");
                    } 

                    serviceIndexList.Add(controllerServiceName.Replace(".ts", ""));
                }

                if (firstTypeList != null && firstTypeList.Count > 0)
                {
                    var firstTypeListDistinct = ", " + Join(", ", firstTypeList.Where(p => p != "void").Distinct().ToArray());
                    serviceFileText.Replace("[firstTypeList]",
                        $"import {{ RestService {firstTypeListDistinct}}} from '@abp/ng.core';");
                }
                else
                {
                    serviceFileText.Replace("[firstTypeList]", "");
                }

                if (secondTypeList != null && secondTypeList.Count > 0)
                {
                    var secondTypeListDistinct = Join(", ", secondTypeList.Where(p => p != "void").Distinct().ToArray());
                    serviceFileText.Replace("[secondTypeList]",
                        $"import {{{secondTypeListDistinct}}} from '../models';");
                }
                else
                {
                    serviceFileText.Replace("[secondTypeList]", "");
                } 

                serviceFileText.AppendLine("}");

                serviceFileText.Replace("[controllerName]", controllerName);
                File.WriteAllText($"src/app/{rootPath}/shared/services/{controllerServiceName}", serviceFileText.ToString());

                var serviceIndexFileText = new StringBuilder();

                foreach (var serviceIndexItem in serviceIndexList)
                {
                    serviceIndexFileText.AppendLine($"export * from './{serviceIndexItem}';");
                }

                File.WriteAllText($"src/app/{rootPath}/shared/services/index.ts", serviceIndexFileText.ToString());

                var modelIndexFileText = new StringBuilder();

                foreach (var modelIndexItem in modelIndexList)
                {
                    modelIndexFileText.AppendLine($"export * from './{modelIndexItem}';");
                }

                File.WriteAllText($"src/app/{rootPath}/shared/models/index.ts", modelIndexFileText.ToString());
            }
        }

        private Dictionary<string, string> GetCombinedModules(JToken data)
        {
            var moduleList = new Dictionary<string, string>();
            foreach (var module in data["modules"])
            {
                var rootPath = ((string)module.First["rootPath"]).ToLower();

                if (moduleList.Any(p => p.Key == rootPath))
                {
                    var value = moduleList[rootPath];

                    moduleList[rootPath] = value.TrimEnd('}') + "," + module.First["controllers"].ToString().TrimStart('{');
                }
                else
                {
                    moduleList.Add(rootPath, module.First["controllers"].ToString());
                }
            }

            return moduleList;
        }

        private string CreateType(JObject data, string returnValueType, string rootPath)
        { 
            var type = data["types"][returnValueType];

            if (returnValueType.StartsWith("Volo.Abp.Application.Dtos")
                 || returnValueType.StartsWith("System.Collections")
                 || returnValueType == "System.String"
                 || returnValueType == "System.Void"
                 || returnValueType.Contains("IActionResult")
                 || returnValueType.Contains("IStringValueType")
                 || returnValueType.Contains("IValueValidator")
                 )
            {
                return null;
            }

            var typeNameSplit = returnValueType.Split(".");
            var typeName = typeNameSplit[typeNameSplit.Length - 1];

            var typeModelName = typeName.Replace("<", "").Replace(">", "").PascalToKebabCase() + ".ts";

            var path = $"src/app/{rootPath}/shared/models/{typeModelName}";
            if (File.Exists(path))
            {
                return null;
            }

            var modelFileText = new StringBuilder(); 

            var baseType = (string)type["baseType"];
            var extends = "";

            if (!IsNullOrWhiteSpace(baseType) && baseType != "System.Enum")
            {
                var baseTypeSplit = baseType.Split(".");
                var baseTypeName = baseTypeSplit[baseTypeSplit.Length - 1].Replace("<", "").Replace(">", "");
                var baseTypeKebabCase = "./" + baseTypeName.PascalToKebabCase();

                if (baseType.Contains("Volo.Abp.Application.Dtos.EntityDto"))
                {
                    baseTypeKebabCase = "@abp/ng.core";
                }

                if (baseTypeName.Contains("guid") || baseTypeName.Contains("Guid"))
                {
                    baseTypeName = "string";
                }

                modelFileText.AppendLine($"import {{ {baseTypeName} }} from '{baseTypeKebabCase}';");
                extends = "extends " + baseTypeName;
            }

            if (baseType == "System.Enum" && (string)type.First["isEnum"] == "True")
            {
                modelFileText.AppendLine($"export enum {typeName} {{");

                var enumNameList = type.First["enumNames"].ToArray();
                var enumValueList = type.First["enumValues"].ToArray();

                for (var i = 0; i < enumNameList.Length; i++)
                {
                    modelFileText.AppendLine($"{enumNameList[i]} = {enumValueList[i]},");
                }

                modelFileText.AppendLine("}");
            }
            else
            {
                modelFileText.AppendLine($"export class {typeName} {extends} {{");

                foreach (var property in type["properties"])
                {
                    var propertyName = (string)property["name"];
                    propertyName = (char.ToLower(propertyName[0]) + propertyName.Substring(1));

                    var typeSimple = (string)property["typeSimple"];

                    if (typeSimple.IndexOf("[") > -1 && typeSimple.IndexOf("]") > -1)
                    {
                        typeSimple = typeSimple.Replace("[", "").Replace("]", "") + "[]";
                    }

                    if (typeSimple.StartsWith("Volo.Abp"))
                    {
                        var typeSimpleSplit = typeSimple.Split(".");
                        typeSimple = typeSimpleSplit[typeSimpleSplit.Length - 1];
                    }

                    if (typeSimple.StartsWith("System.Object"))
                    {
                        typeSimple = typeSimple.Replace("System.Object", "object");
                    }

                    if (typeSimple.Contains("?"))
                    {
                        typeSimple = typeSimple.Replace("?", "");
                        propertyName += "?";
                    }

                    if (typeSimple != "boolean"
                     && typeSimple != "string"
                     && typeSimple != "number"
                     && typeSimple != "boolean[]"
                     && typeSimple != "string[]"
                     && typeSimple != "number[]"
                      )
                    {
                        typeSimple = "any" + (typeSimple.Contains("[]") ? "[]" : "");
                    }

                    modelFileText.AppendLine($"  {propertyName}: {typeSimple};");
                }

                modelFileText.AppendLine("");

                modelFileText.AppendLine($"  constructor(initialValues: Partial<{typeName}> = {{}}) {{");

                if (!IsNullOrWhiteSpace(baseType))
                {
                    modelFileText.AppendLine("    super(initialValues);");
                    modelFileText.AppendLine("  }");
                }
                else
                {
                    modelFileText.AppendLine("    if (initialValues) {");
                    modelFileText.AppendLine("      for (const key in initialValues) {");
                    modelFileText.AppendLine("        if (initialValues.hasOwnProperty(key)) {");
                    modelFileText.AppendLine("          this[key] = initialValues[key];");

                    modelFileText.AppendLine("        }");
                    modelFileText.AppendLine("      }");
                    modelFileText.AppendLine("    }");
                    modelFileText.AppendLine("  }");
                }

                modelFileText.AppendLine("}");
            }

            File.WriteAllText($"src/app/{rootPath}/shared/models/{typeModelName}", modelFileText.ToString());

            return typeModelName.Replace(".ts", "");
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy --apiUrl <api-url> [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-u|--ui <ui-framework>               (default: angular)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp new --apiUrl https://www.abp.io/api/abp/api-definition?types=true");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Generates a ...";
        }

        private UiFramework GetUiFramework(CommandLineArgs commandLineArgs)
        {
            var optionValue = commandLineArgs.Options.GetOrNull(Options.UiFramework.Short, Options.UiFramework.Long);
            switch (optionValue)
            {
                case "none":
                    return UiFramework.None;
                case "mvc":
                    return UiFramework.Mvc;
                case "angular":
                    return UiFramework.Angular;
                default:
                    return UiFramework.NotSpecified;
            }
        }

        public static class Options
        {
            public static class ApiUrl
            {
                public const string Short = "au";
                public const string Long = "apiUrl";
            }

            public static class UiFramework
            {
                public const string Short = "u";
                public const string Long = "ui";
            }
        }
    }

    public static class StringExtensions
    {
        public static string PascalToKebabCase(this string value)
        {
            if (IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}