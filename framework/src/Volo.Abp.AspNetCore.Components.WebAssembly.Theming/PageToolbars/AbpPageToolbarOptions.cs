using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.PageToolbars
{
    public class AbpPageToolbarOptions
    {
        public PageToolbarDictionary Toolbars { get; }

        public AbpPageToolbarOptions()
        {
            Toolbars = new PageToolbarDictionary();
        }

        public void Configure<TPage>([NotNull] Action<PageToolbar> configureAction)
        {
            Configure(typeof(TPage).FullName, configureAction);
        }

        public void Configure([NotNull] string pageName, [NotNull] Action<PageToolbar> configureAction)
        {
            Check.NotNullOrWhiteSpace(pageName, nameof(pageName));
            Check.NotNull(configureAction, nameof(configureAction));

            var toolbar = Toolbars.GetOrAdd(pageName, () => new PageToolbar(pageName));
            configureAction(toolbar);
        }
    }
}
