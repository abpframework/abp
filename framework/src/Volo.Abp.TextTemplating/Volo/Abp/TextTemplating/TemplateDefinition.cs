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

        public TemplateContentContributorList ContentContributors { get; }

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
            ContentContributors = new TemplateContentContributorList();
            IsLayout = isLayout;
            Layout = layout;
            DefaultCultureName = defaultCultureName;
        }

        public virtual TemplateDefinition WithContributor(ITemplateContentContributor contentContributor)
        {
            ContentContributors.Add(contentContributor);
            return this;
        }
    }
}