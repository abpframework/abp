using System;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public class TemplateDefinition
    {
        public const string DefaultLayoutPlaceHolder = "_";

        [NotNull]
        public string Name { get; }

        public bool IsLayout { get; }

        [CanBeNull]
        public string Layout { get; set; }

        public Type LocalizationResource { get; set; } //TODO: ???

        public TemplateContributorList Contributors { get; }

        [CanBeNull]
        public string DefaultCultureName { get; }

        public bool SingleTemplateFile { get; } //TODO: ???

        public TemplateDefinition(
            [NotNull] string name, 
            Type localizationResource = null, 
            bool isLayout = false,
            string layout = DefaultLayoutPlaceHolder, 
            string defaultCultureName = null, 
            bool singleTemplateFile = false)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            LocalizationResource = localizationResource;
            Contributors = new TemplateContributorList();
            IsLayout = isLayout;
            Layout = layout;
            DefaultCultureName = defaultCultureName;
            SingleTemplateFile = singleTemplateFile;
        }
    }
}