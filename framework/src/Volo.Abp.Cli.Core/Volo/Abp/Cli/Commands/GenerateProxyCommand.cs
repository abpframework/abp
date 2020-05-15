using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ProjectBuilding;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Volo.Abp.Cli.Commands
{
    public class GenerateProxyCommand : IConsoleCommand, ITransientDependency
    {
        public static Dictionary<string, Dictionary<string, string>> propertyList = new Dictionary<string, Dictionary<string, string>>();
        public ILogger<GenerateProxyCommand> Logger { get; set; }
        public static string outputPrefix = "src/app";
        public static string output = "";

        protected TemplateProjectBuilder TemplateProjectBuilder { get; }

        public GenerateProxyCommand(TemplateProjectBuilder templateProjectBuilder)
        {
            TemplateProjectBuilder = templateProjectBuilder;

            Logger = NullLogger<GenerateProxyCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            var angularPath = $"angular.json";
            if (!File.Exists(angularPath))
            {
                throw new CliUsageException(
                           "angular.json file not found. You must run this command in angular folder." +
                           Environment.NewLine + Environment.NewLine +
                           GetUsageInfo()
                       );
            }

            var module = commandLineArgs.Options.GetOrNull(Options.Module.Short, Options.Module.Long);
            module = module == null ? "app" : module.ToLower();

            var apiUrl = commandLineArgs.Options.GetOrNull(Options.ApiUrl.Short, Options.ApiUrl.Long);
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                var environmentJson = File.ReadAllText("src/environments/environment.ts").Split("export const environment = ")[1].Replace(";", " ");
                var environment = JObject.Parse(environmentJson);
                apiUrl = environment["apis"]["default"]["url"].ToString();
            }
            apiUrl += "/api/abp/api-definition?IncludeTypes=true";

            var uiFramework = GetUiFramework(commandLineArgs);

            output = commandLineArgs.Options.GetOrNull(Options.Output.Short, Options.Output.Long);
            if (!string.IsNullOrWhiteSpace(output) && !(output.EndsWith("/") || output.EndsWith("\\")))
            { 
                output += "/";
            }

            WebClient client = new WebClient();
            string json = "";
            try
            {
                json = client.DownloadString(apiUrl);
            }
            catch (Exception ex)
            {
                throw new CliUsageException(
                           "Cannot connect to the host {" + apiUrl + "}! Check that the host is up and running." +
                           Environment.NewLine + Environment.NewLine +
                           GetUsageInfo()
                       );
            }

            Logger.LogInformation("Downloading api definition...");
            Logger.LogInformation("Api Url: " + apiUrl);

            var data = JObject.Parse(json);

            Logger.LogInformation("Modules are combining");
            var apiNameList = new Dictionary<string, string>();
            var moduleList = GetCombinedModules(data, module, out apiNameList);

            if (moduleList.Count < 1)
            {
                throw new CliUsageException(
                        "Module can not find!" +
                        Environment.NewLine + Environment.NewLine +
                        GetUsageInfo()
                    );
            }

            Logger.LogInformation("Modules and types are creating");

            foreach (var moduleItem in moduleList)
            {
                var moduleValue = JObject.Parse(moduleItem.Value);

                var rootPath = moduleItem.Key;
                var apiName = apiNameList.Where(p => p.Key == rootPath).Select(p => p.Value).FirstOrDefault();

                if (rootPath != "app")
                {
                    Logger.LogInformation($"{rootPath} directory is creating");
                }                

                if (rootPath == "app")
                {
                    outputPrefix = "src";
                }
                else
                {
                    outputPrefix = "src/app";
                }

                Directory.CreateDirectory(output + $"{outputPrefix}/{rootPath}");

                foreach (var controller in moduleValue.Root.ToList().Select(item => item.First))
                {
                    var serviceIndexList = new List<string>();
                    var modelIndexList = new List<string>();

                    var serviceFileText = new StringBuilder();

                    serviceFileText.AppendLine("[firstTypeList]");
                    serviceFileText.AppendLine("import { Injectable } from '@angular/core';");
                    serviceFileText.AppendLine("import { Observable } from 'rxjs';");
                    serviceFileText.AppendLine("[secondTypeList]");
                    serviceFileText.AppendLine("");
                    serviceFileText.AppendLine("@Injectable({providedIn: 'root'})");
                    serviceFileText.AppendLine("export class [controllerName]Service {");
                    serviceFileText.AppendLine("  apiName = '" + apiName + "';");
                    serviceFileText.AppendLine("");
                    serviceFileText.AppendLine("  constructor(private restService: RestService) {}");
                    serviceFileText.AppendLine("");

                    var firstTypeList = new List<string>();
                    var secondTypeList = new List<string>();

                    var controllerName = (string)controller["controllerName"];
                    var controllerServiceName = controllerName.PascalToKebabCase() + ".service.ts";

                    var controllerPathName = controllerName.ToLower().Replace("controller", "");
                    controllerPathName = (controllerPathName.StartsWith(rootPath)) ? controllerPathName.Substring(rootPath.Length) : controllerPathName;

                    Directory.CreateDirectory(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models");
                    Directory.CreateDirectory(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/services");

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
                        var parameterModel = new List<ParameterModel>();

                        foreach (var parameter in parameters.OrderBy(p => p["bindingSourceId"]))
                        {
                            var bindingSourceId = (string)parameter["bindingSourceId"];
                            bindingSourceId = char.ToLower(bindingSourceId[0]) + bindingSourceId.Substring(1);

                            var name = (string)parameter["name"];
                            var typeSimple = (string)parameter["typeSimple"];
                            var typeArray = ((string)parameter["type"]).Split(".");
                            var type = (typeArray[typeArray.Length - 1]).TrimEnd('>');
                            var isOptional = (bool)parameter["isOptional"];
                            var defaultValue = (string)parameter["defaultValue"];

                            var modelIndex = CreateType(data, (string)parameter["type"], rootPath, modelIndexList, controllerPathName);

                            if (!string.IsNullOrWhiteSpace(modelIndex))
                            {
                                modelIndexList.Add(modelIndex);
                            }

                            if (bindingSourceId == "body")
                            {
                                bodyExtra = ", body";
                                parameterModel = AddParameter(bindingSourceId, type, isOptional, defaultValue, bindingSourceId, parameterModel);
                            }
                            else if (bindingSourceId == "path")
                            {
                                parameterModel = AddParameter(name, typeSimple, isOptional, defaultValue, bindingSourceId, parameterModel);
                            }
                            else if (bindingSourceId == "modelBinding")
                            {
                                var parameterNameOnMethod = (string)parameter["nameOnMethod"];

                                var parametersOnMethod = action["parametersOnMethod"];
                                foreach (var parameterOnMethod in parametersOnMethod)
                                {
                                    var parametersOnMethodName = (string)parameterOnMethod["name"];
                                    if (parametersOnMethodName == parameterNameOnMethod)
                                    {
                                        typeSimple = (string)parameterOnMethod["typeSimple"];
                                        typeArray = ((string)parameterOnMethod["type"]).Split(".");
                                        type = typeArray[typeArray.Length - 1];
                                        isOptional = (bool)parameterOnMethod["isOptional"];
                                        defaultValue = (string)parameterOnMethod["defaultValue"];

                                        if (typeSimple == "string" || typeSimple == "boolean" || typeSimple == "number")
                                        {
                                            parameterModel = AddParameter(name, typeSimple, isOptional, defaultValue, bindingSourceId, parameterModel);
                                        }

                                        modelIndex = CreateType(data, (string)parameterOnMethod["type"], rootPath, modelIndexList, controllerPathName);

                                        if (!string.IsNullOrWhiteSpace(modelIndex))
                                        {
                                            modelIndexList.Add(modelIndex);
                                        }
                                    }
                                }

                                if (typeSimple != "string" && typeSimple != "boolean" && typeSimple != "number")
                                {
                                    parametersText.Append($"params = {{}} as {type}");
                                    modelBindingExtra = ", params";
                                    if (!string.IsNullOrWhiteSpace(modelIndex))
                                    {
                                        secondTypeList.Add(type);
                                    }
                                    else
                                    {
                                        firstTypeList.Add(type);
                                    }
                                    break;
                                }
                            }
                        }

                        if (parameterModel != null && parameterModel.Count > 0)
                        {
                            foreach (var parameterItem in parameterModel.OrderBy(p => p.DisplayOrder))
                            {
                                var parameterItemModelName = parameterItem.Type.PascalToKebabCase() + ".ts";
                                var parameterItemModelPath = output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{parameterItemModelName}";
                                if (parameterItem.BindingSourceId == "body" && !File.Exists(parameterItemModelPath))
                                {
                                    parameterItem.Type = "any";
                                }

                                parametersIndex++;

                                if (parametersIndex > 1)
                                {
                                    parametersText.Append(", ");
                                }

                                parametersText.Append(parameterItem.Name + (parameterItem.IsOptional ? "?" : "") + ": " + parameterItem.Type + (parameterItem.Value != null ? (" = " + (string.IsNullOrWhiteSpace(parameterItem.Value) ? "''" : parameterItem.Value)) : ""));

                                if (parameterItem.BindingSourceId == "modelBinding")
                                {
                                    modelBindingExtraList.Add(parameterItem.Name);
                                }
                                else if (parameterItem.BindingSourceId == "body" && File.Exists(parameterItemModelPath))
                                {
                                    secondTypeList.Add(parameterItem.Type);
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

                                var secondTypeModelName = secondType.PascalToKebabCase() + ".ts";
                                var secondTypeModelPath = output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{secondTypeModelName}";
                                if (firstType == "List" && !File.Exists(secondTypeModelPath))
                                {
                                    secondType = "any";
                                }

                                serviceFileText.AppendLine(
                                    firstType == "List"
                                        ? $" {actionName}({parametersText}): Observable<{secondType}[]> {{"
                                        : $" {actionName}({parametersText}): Observable<{firstType}<{secondType}>> {{");

                                if (firstType != "List")
                                {
                                    firstTypeList.Add(firstType);
                                }

                                if (secondType != "any")
                                {
                                    secondTypeList.Add(secondType);
                                }
                            }
                            else
                            {
                                var typeArray = returnValueType.Split(".");
                                var type = typeArray[typeArray.Length - 1].TrimEnd('>');

                                type = type switch
                                {
                                    "Void" => "void",
                                    "String" => "string",
                                    "IActionResult" => "void",
                                    "ActionResult" => "void",
                                    _ => type
                                };

                                serviceFileText.AppendLine(
                                    $" {actionName}({parametersText}): Observable<{type}> {{");

                                if (type != "void" && type != "string")
                                {
                                    secondTypeList.Add(type);
                                }
                            }

                            var modelIndex = CreateType(data, returnValueType, rootPath, modelIndexList, controllerPathName);

                            if (!string.IsNullOrWhiteSpace(modelIndex))
                            {
                                modelIndexList.Add(modelIndex);
                            }
                        }

                        if (modelBindingExtraList != null && modelBindingExtraList.Count > 0)
                        {
                            modelBindingExtra = ", params: { " + string.Join(", ", modelBindingExtraList.ToArray()) + " }";
                        }

                        var url = ((string)action["url"]).Replace("/{", "/${");
                        var httpMethod = (string)action["httpMethod"];

                        serviceFileText.AppendLine(
                            url.Contains("${")
                                ? $"   return this.restService.request({{ url: `/{url}`, method: '{httpMethod}'{bodyExtra}{modelBindingExtra} }},{{ apiName: this.apiName }});"
                                : $"   return this.restService.request({{ url: '/{url}', method: '{httpMethod}'{bodyExtra}{modelBindingExtra} }},{{ apiName: this.apiName }});");


                        serviceFileText.AppendLine(" }");
                    }

                    serviceIndexList.Add(controllerServiceName.Replace(".ts", ""));

                    if (firstTypeList != null && firstTypeList.Count > 0)
                    {
                        var firstTypeListDistinct = ", " + string.Join(", ", firstTypeList.Where(p => p != "void").Distinct().ToArray());
                        serviceFileText.Replace("[firstTypeList]",
                            $"import {{ RestService {firstTypeListDistinct}}} from '@abp/ng.core';");
                    }
                    else
                    {
                        serviceFileText.Replace("[firstTypeList]",
                            $"import {{ RestService }} from '@abp/ng.core';");
                    }

                    if (secondTypeList != null && secondTypeList.Count > 0)
                    {
                        var secondTypeListDistinct = string.Join(", ", secondTypeList.Where(p => p != "void").Distinct().ToArray());
                        serviceFileText.Replace("[secondTypeList]",
                            $"import {{{secondTypeListDistinct}}} from '../models';");
                    }
                    else
                    {
                        serviceFileText.Replace("[secondTypeList]", "");
                    }

                    serviceFileText.AppendLine("}");

                    serviceFileText.Replace("[controllerName]", controllerName);
                    File.WriteAllText(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/services/{controllerServiceName}", serviceFileText.ToString());



                    var serviceIndexFileText = new StringBuilder();

                    foreach (var serviceIndexItem in serviceIndexList.Distinct())
                    {
                        serviceIndexFileText.AppendLine($"export * from './{serviceIndexItem}';");
                    }

                    File.WriteAllText(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/services/index.ts", serviceIndexFileText.ToString());

                    if (modelIndexList.Count > 0)
                    {
                        var modelIndexFileText = new StringBuilder();

                        foreach (var modelIndexItem in modelIndexList.Distinct())
                        {
                            modelIndexFileText.AppendLine($"export * from './{modelIndexItem}';");
                        }

                        File.WriteAllText(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/index.ts", modelIndexFileText.ToString());
                    }
                }
            }

            Logger.LogInformation("Completed!");
        }

        private Dictionary<string, string> GetCombinedModules(JToken data, string module, out Dictionary<string, string> apiNameList)
        {
            var moduleList = new Dictionary<string, string>();
            apiNameList = new Dictionary<string, string>();

            foreach (var moduleItem in data["modules"])
            {
                var rootPath = ((string)moduleItem.First["rootPath"]).ToLower();
                var apiName = (string)moduleItem.First["remoteServiceName"];

                if (moduleList.Any(p => p.Key == rootPath))
                {
                    var value = moduleList[rootPath];

                    moduleList[rootPath] = value.TrimEnd('}') + "," + moduleItem.First["controllers"].ToString().TrimStart('{');
                }
                else
                {
                    apiNameList.Add(rootPath, apiName);
                    moduleList.Add(rootPath, moduleItem.First["controllers"].ToString());
                }
            }

            if (module != "all")
            {
                moduleList = moduleList.Where(p => p.Key.ToLower() == module).ToDictionary(p => p.Key, s => s.Value);
            }

            return moduleList;
        }

        private static string CreateType(JObject data, string returnValueType, string rootPath, List<string> modelIndexList, string controllerPathName)
        {
            var type = data["types"][returnValueType];

            if (type == null)
            {
                return null;
            }

            if (returnValueType.StartsWith("Volo.Abp.Application.Dtos")
             || returnValueType.StartsWith("System.Collections")
             || returnValueType == "System.String"
             || returnValueType == "System.Void"
             || returnValueType.Contains("System.Net.HttpStatusCode?")
             || returnValueType.Contains("IActionResult")
             || returnValueType.Contains("ActionResult")
             || returnValueType.Contains("IStringValueType")
             || returnValueType.Contains("IValueValidator")
               )
            {
                if (returnValueType.Contains("<"))
                {
                    returnValueType = returnValueType.Split('<')[1].Split('>')[0];
                    if (returnValueType.StartsWith("Volo.Abp.Application.Dtos")
                     || returnValueType.StartsWith("System.Collections")
                     || returnValueType == "System.String"
                     || returnValueType == "System.Void"
                     || returnValueType.Contains("System.Net.HttpStatusCode?")
                     || returnValueType.Contains("IActionResult")
                     || returnValueType.Contains("ActionResult")
                     || returnValueType.Contains("IStringValueType")
                     || returnValueType.Contains("IValueValidator")
                     || returnValueType.Contains("Guid")
                       )
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            var typeNameSplit = returnValueType.Split(".");
            var typeName = typeNameSplit[typeNameSplit.Length - 1];

            var typeModelName = typeName.Replace("<", "").Replace(">", "").Replace("?", "").PascalToKebabCase() + ".ts";

            var path = output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{typeModelName}";

            var modelFileText = new StringBuilder();

            var baseType = (string)type["baseType"];
            var extends = "";
            var customBaseTypeName = "";
            var baseTypeName = "";

            if (!string.IsNullOrWhiteSpace(baseType))
            {
                var baseTypeSplit = baseType.Split(".");
                baseTypeName = baseTypeSplit[baseTypeSplit.Length - 1].Replace("<", "").Replace(">", "");
                var baseTypeKebabCase = "./" + baseTypeName.PascalToKebabCase();

                if (baseType != "System.Enum")
                {
                    if (baseType.Contains("Volo.Abp.Application.Dtos"))
                    {
                        baseTypeKebabCase = "@abp/ng.core";

                        baseTypeName = baseType.Split("Volo.Abp.Application.Dtos")[1].Split("<")[0].TrimStart('.');
                        customBaseTypeName = baseType.Split("Volo.Abp.Application.Dtos")[1].Replace("System.Guid", "string").TrimStart('.');
                    }

                    if (baseTypeName.Contains("guid") || baseTypeName.Contains("Guid"))
                    {
                        baseTypeName = "string";
                    }

                    modelFileText.AppendLine(Environment.NewLine);
                    modelFileText.AppendLine($"import {{ {baseTypeName} }} from '{baseTypeKebabCase}';");
                    
                    extends = "extends " + (!string.IsNullOrWhiteSpace(customBaseTypeName) ? customBaseTypeName : baseTypeName);

                    var modelIndex = CreateType(data, baseType, rootPath, modelIndexList, controllerPathName);
                    if (!string.IsNullOrWhiteSpace(modelIndex))
                    {
                        modelIndexList.Add(modelIndex);
                    }
                }
            }

            if (baseType == "System.Enum" && (bool)type["isEnum"])
            {
                modelFileText.AppendLine($"export enum {typeName} {{");

                var enumNameList = type["enumNames"].ToArray();
                var enumValueList = type["enumValues"].ToArray();

                for (var i = 0; i < enumNameList.Length; i++)
                {
                    modelFileText.AppendLine($"    {enumNameList[i]} = {enumValueList[i]},");
                }

                modelFileText.AppendLine("}");
            }
            else
            {
                modelFileText.AppendLine("");
                modelFileText.AppendLine($"export class {typeName} {extends} {{");

                foreach (var property in type["properties"])
                {
                    var propertyName = (string)property["name"];
                    propertyName = (char.ToLower(propertyName[0]) + propertyName.Substring(1));
                    var typeSimple = (string)property["typeSimple"];

                    var modelIndex = CreateType(data, (string)property["type"], rootPath, modelIndexList, controllerPathName);

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
                        var typeSimpleModelName = typeSimple.PascalToKebabCase() + ".ts";
                        var modelPath = output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{typeSimpleModelName}";
                        if (!File.Exists(modelPath))
                        {
                            typeSimple = "any" + (typeSimple.Contains("[]") ? "[]" : "");
                        }
                    }

                    if (propertyList.Any(p => p.Key == baseTypeName && p.Value.Any(q => q.Key == propertyName && q.Value == typeSimple)))
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(modelIndex))
                    {
                        var from = "../models";
                        var propertyTypeSplit = ((string)property["type"]).Split(".");
                        var propertyType = propertyTypeSplit[propertyTypeSplit.Length - 1];

                        var propertyTypeKebabCase = propertyType.PascalToKebabCase();
                        if (File.Exists(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{propertyTypeKebabCase}.ts"))
                        {
                            from = "./" + propertyTypeKebabCase;
                        }
                         
                        modelFileText.Insert(0, $"import {{ {propertyType} }} from '{from}';");
                        modelFileText.Insert(0, Environment.NewLine);
                        modelIndexList.Add(modelIndex);
                    }

                    if (propertyList.Any(p => p.Key == typeName && !p.Value.Any(q => q.Key == propertyName)))
                    {
                        propertyList[typeName].Add(propertyName, typeSimple);
                    }
                    else if (!propertyList.Any(p => p.Key == typeName))
                    {
                        var newProperty = new Dictionary<string, string>();
                        newProperty.Add(propertyName, typeSimple);
                        propertyList.Add(typeName, newProperty);
                    }

                    modelFileText.AppendLine($"  {propertyName}: {typeSimple};");
                }

                modelFileText.AppendLine("");

                modelFileText.AppendLine($"  constructor(initialValues: Partial<{typeName}> = {{}}) {{");

                if (!string.IsNullOrWhiteSpace(baseType))
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

            File.WriteAllText(output + $"{outputPrefix}/{rootPath}/{controllerPathName}/models/{typeModelName}", modelFileText.ToString());

            return typeModelName.Replace(".ts", "");
        }

        private static List<ParameterModel> AddParameter(string parameterName, string type, bool parameterIsOptional, string parameterDefaultValue, string bindingSourceId, List<ParameterModel> parameterModel)
        {
            if (parameterDefaultValue != "null")
            {
                parameterModel.Add(new ParameterModel
                {
                    DisplayOrder = 3,
                    Name = parameterName,
                    Type = type,
                    Value = parameterDefaultValue,
                    BindingSourceId = bindingSourceId
                });
            }
            else if (parameterDefaultValue == "null" && parameterIsOptional)
            {
                parameterModel.Add(new ParameterModel
                {
                    DisplayOrder = 2,
                    Name = parameterName,
                    IsOptional = true,
                    Type = type,
                    BindingSourceId = bindingSourceId
                });
            }
            else
            {
                parameterModel.Add(new ParameterModel
                {
                    DisplayOrder = 1,
                    Name = parameterName,
                    Type = type,
                    BindingSourceId = bindingSourceId
                });
            }

            return parameterModel;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy [options]");
            sb.AppendLine("");
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-a|--apiUrl <api-url>               (default: environment.ts>apis>default>url)");
            sb.AppendLine("-u|--ui <ui-framework>               (default: angular)");
            sb.AppendLine("-m|--module <module>               (default: app)");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy --apiUrl https://www.volosoft.com");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Generates typescript service proxies and DTOs";
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
                    return UiFramework.Angular;
            }
        }

        public static class Options
        {
            public static class Module
            {
                public const string Short = "m";
                public const string Long = "module";
            }

            public static class ApiUrl
            {
                public const string Short = "a";
                public const string Long = "apiUrl";
            }

            public static class UiFramework
            {
                public const string Short = "u";
                public const string Long = "ui";
            }
            public static class Output
            {
                public const string Short = "o";
                public const string Long = "out";
            }
        }
    }

    public static class StringExtensions
    {
        public static string PascalToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }

    public class ParameterModel
    {
        public int DisplayOrder { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool IsOptional { get; set; }
        public string BindingSourceId { get; set; }
    }
}