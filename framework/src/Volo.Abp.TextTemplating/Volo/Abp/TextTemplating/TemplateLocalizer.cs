using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;

namespace Volo.Abp.TextTemplating
{
    public class TemplateLocalizer : IScriptCustomFunction
    {
        private readonly IStringLocalizer _localizer;

        public TemplateLocalizer(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

        public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments,
            ScriptBlockStatement blockStatement)
        {
            return GetString(arguments);
        }

        public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments,
            ScriptBlockStatement blockStatement)
        {
            return new ValueTask<object>(GetString(arguments));
        }

        private string GetString(ScriptArray arguments)
        {
            if (arguments.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var name = arguments[0];
            if (name == null || name.ToString().IsNullOrWhiteSpace())
            {
                return string.Empty;
            }

            var args = arguments.Skip(1).Where(x => x != null && !x.ToString().IsNullOrWhiteSpace()).ToArray();
            return args.Any() ? _localizer[name.ToString(), args] : _localizer[name.ToString()];
        }

        public int RequiredParameterCount => 1;

        public int ParameterCount => ScriptFunctionCall.MaximumParameterCount - 1;

        public ScriptVarParamKind VarParamKind => ScriptVarParamKind.Direct;

        public Type ReturnType => typeof(object);

        public ScriptParameterInfo GetParameterInfo(int index)
        {
            return index == 0
                ? new ScriptParameterInfo(typeof(string), "template_name")
                : new ScriptParameterInfo(typeof(object), "value");
        }
    }
}
