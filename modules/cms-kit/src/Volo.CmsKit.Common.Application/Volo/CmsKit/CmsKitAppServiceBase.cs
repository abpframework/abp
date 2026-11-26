using System;
using System.Linq;
using Volo.Abp.Application.Services;
using Volo.Abp.Validation;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

public abstract class CmsKitAppServiceBase : ApplicationService
{
    protected CmsKitAppServiceBase()
    {
        LocalizationResource = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitCommonApplicationModule);
    }

    private static readonly string[] SortingDirections = { "asc", "desc", "ascending", "descending" };

    protected virtual void CheckSorting(string sorting, string[] allowedFields)
    {
        if (sorting.IsNullOrWhiteSpace())
        {
            return;
        }

        foreach (var expression in sorting.Split(','))
        {
            var parts = expression.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var isValid = parts.Length is 1 or 2 &&
                          allowedFields.Contains(parts[0], StringComparer.OrdinalIgnoreCase) &&
                          (parts.Length == 1 || SortingDirections.Contains(parts[1], StringComparer.OrdinalIgnoreCase));
            if (!isValid)
            {
                throw new AbpValidationException("Sorting expression is not supported.");
            }
        }
    }
}
