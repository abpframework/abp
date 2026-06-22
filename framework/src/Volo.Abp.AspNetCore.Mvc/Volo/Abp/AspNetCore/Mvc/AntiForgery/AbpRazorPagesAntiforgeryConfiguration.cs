using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.AspNetCore.Mvc.AntiForgery;

public static class AbpRazorPagesAntiforgeryConfiguration
{
    public static void Configure(RazorPagesOptions options)
    {
        options.Conventions.AddFolderApplicationModelConvention("/", ReplaceAntiforgeryFilter);
    }

    private static void ReplaceAntiforgeryFilter(PageApplicationModel model)
    {
        for (var i = model.Filters.Count - 1; i >= 0; i--)
        {
            if (model.Filters[i] is AutoValidateAntiforgeryTokenAttribute)
            {
                model.Filters.RemoveAt(i);
            }
        }

        if (!model.Filters.Any(f => f is AbpAutoValidateAntiforgeryTokenAttribute))
        {
            model.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
        }
    }
}
