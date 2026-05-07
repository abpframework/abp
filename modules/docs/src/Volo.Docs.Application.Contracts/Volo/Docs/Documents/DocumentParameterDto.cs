using System.Collections.Generic;

namespace Volo.Docs.Documents
{
    public class DocumentParameterDto
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Dictionary<string, string> Values { get; set; }

        /// <summary>
        /// Conditional visibility: this parameter is shown only when the keyed parameter's
        /// current value is one of the listed values.
        /// Example: "DependsOn": { "UI": [ "Blazor", "BlazorServer", "BlazorWebApp" ] }
        /// An empty dictionary means the parameter is always shown.
        /// </summary>
        public Dictionary<string, List<string>> DependsOn { get; set; } = new Dictionary<string, List<string>>();
    }
}