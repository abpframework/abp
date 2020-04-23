using System;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public class TemplateDefinition
    {
        [NotNull]
        public string Name { get; }

        public bool IsLayout { get; }

        [CanBeNull]
        public string Layout { get; set; }

        [CanBeNull]
        public Type LocalizationResource { get; set; }

        public TemplateContributorList Contributors { get; }

        [CanBeNull]
        public string DefaultCultureName { get; }

        public TemplateDefinition(
            [NotNull] string name, 
            [CanBeNull] Type localizationResource = null, 
            bool isLayout = false,
            string layout = null, 
            string defaultCultureName = null)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            LocalizationResource = localizationResource;
            Contributors = new TemplateContributorList();
            IsLayout = isLayout;
            Layout = layout;
            DefaultCultureName = defaultCultureName;
        }

        public virtual TemplateDefinition AddContributor(ITemplateContributor contributor)
        {
            Contributors.Add(contributor);
            return this;
        }
    }
}