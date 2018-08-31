using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateDefinition
    {
        public string Name { get; }

        public Dictionary<string, object> Properties { get; }

        /// <summary>
        /// Gets/sets a key-value on the <see cref="Properties"/>.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <returns>
        /// Returns the value in the <see cref="Properties"/> dictionary by given <see cref="name"/>.
        /// Returns null if given <see cref="name"/> is not present in the <see cref="Properties"/> dictionary.
        /// </returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        public EmailTemplateDefinition([NotNull]string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Properties = new Dictionary<string, object>();
        }
    }
}