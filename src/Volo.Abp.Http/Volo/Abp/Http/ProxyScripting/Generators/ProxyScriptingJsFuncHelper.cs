using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.ProxyScripting.Generators
{
    internal static class ProxyScriptingJsFuncHelper
    {
        private const string ValidJsVariableNameChars = "abcdefghijklmnopqrstuxwvyzABCDEFGHIJKLMNOPQRSTUXWVYZ0123456789_";

        private static readonly HashSet<string> ReservedWords = new HashSet<string> {
            "abstract",
            "else",
            "instanceof",
            "super",
            "boolean",
            "enum",
            "int",
            "switch",
            "break",
            "export",
            "interface",
            "synchronized",
            "byte",
            "extends",
            "let",
            "this",
            "case",
            "false",
            "long",
            "throw",
            "catch",
            "final",
            "native",
            "throws",
            "char",
            "finally",
            "new",
            "transient",
            "class",
            "float",
            "null",
            "true",
            "const",
            "for",
            "package",
            "try",
            "continue",
            "function",
            "private",
            "typeof",
            "debugger",
            "goto",
            "protected",
            "var",
            "default",
            "if",
            "public",
            "void",
            "delete",
            "implements",
            "return",
            "volatile",
            "do",
            "import",
            "short",
            "while",
            "double",
            "in",
            "static",
            "with"
        };

        public static string NormalizeJsVariableName(string name, string additionalChars = "")
        {
            var validChars = ValidJsVariableNameChars + additionalChars;

            var sb = new StringBuilder(name);

            sb.Replace('-', '_');

            //Delete invalid chars
            foreach (var c in name)
            {
                if (!validChars.Contains(c))
                {
                    sb.Replace(c.ToString(), "");
                }
            }

            if (sb.Length == 0)
            {
                return "_" + Guid.NewGuid().ToString("N").Left(8);
            }

            return sb.ToString();
        }

        public static string WrapWithBracketsOrWithDotPrefix(string name)
        {
            if (!ReservedWords.Contains(name))
            {
                return "." + name;
            }

            return "['" + name + "']";
        }

        public static string GetParamNameInJsFunc(ParameterApiDescriptionModel parameterInfo)
        {
            return parameterInfo.Name == parameterInfo.NameOnMethod
                       ? NormalizeJsVariableName(parameterInfo.Name.ToCamelCase(), ".")
                       : NormalizeJsVariableName(parameterInfo.NameOnMethod.ToCamelCase()) + "." + NormalizeJsVariableName(parameterInfo.Name.ToCamelCase(), ".");
        }

        public static string CreateJsObjectLiteral(ParameterApiDescriptionModel[] parameters, int indent = 0)
        {
            var sb = new StringBuilder();

            sb.AppendLine("{");

            foreach (var prm in parameters)
            {
                sb.AppendLine($"{new string(' ', indent)}  '{prm.Name}': {GetParamNameInJsFunc(prm)}");
            }

            sb.Append(new string(' ', indent) + "}");

            return sb.ToString();
        }

        public static string GenerateJsFuncParameterList(ActionApiDescriptionModel action, string ajaxParametersName)
        {
            var methodParamNames = action.Parameters.Select(p => p.NameOnMethod).Distinct().ToList();
            methodParamNames.Add(ajaxParametersName);
            return methodParamNames.Select(prmName => NormalizeJsVariableName(prmName.ToCamelCase())).JoinAsString(", ");
        }
    }
}