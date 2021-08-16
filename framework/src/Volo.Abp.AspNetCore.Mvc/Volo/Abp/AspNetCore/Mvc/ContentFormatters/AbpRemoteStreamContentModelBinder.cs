using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters
{
    public class AbpRemoteStreamContentModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var postedFiles = new List<IRemoteStreamContent>();

            // If we're at the top level, then use the FieldName (parameter or property name).
            // This handles the fact that there will be nothing in the ValueProviders for this parameter
            // and so we'll do the right thing even though we 'fell-back' to the empty prefix.
            var modelName = bindingContext.IsTopLevelObject
                ? bindingContext.BinderModelName ?? bindingContext.FieldName
                : bindingContext.ModelName;

            await GetFormFilesAsync(modelName, bindingContext, postedFiles);

            // If ParameterBinder incorrectly overrode ModelName, fall back to OriginalModelName prefix. Comparisons
            // are tedious because e.g. top-level parameter or property is named Blah and it contains a BlahBlah
            // property. OriginalModelName may be null in tests.
            if (postedFiles.Count == 0 &&
                bindingContext.OriginalModelName != null &&
                !string.Equals(modelName, bindingContext.OriginalModelName, StringComparison.Ordinal) &&
                !modelName.StartsWith(bindingContext.OriginalModelName + "[", StringComparison.Ordinal) &&
                !modelName.StartsWith(bindingContext.OriginalModelName + ".", StringComparison.Ordinal))
            {
                modelName = ModelNames.CreatePropertyModelName(bindingContext.OriginalModelName, modelName);
                await GetFormFilesAsync(modelName, bindingContext, postedFiles);
            }

            object value;
            if (bindingContext.ModelType == typeof(IRemoteStreamContent) || bindingContext.ModelType == typeof(RemoteStreamContent))
            {
                if (postedFiles.Count == 0)
                {
                    // Silently fail if the named file does not exist in the request.
                    return;
                }

                value = postedFiles.First();
            }
            else
            {
                if (postedFiles.Count == 0 && !bindingContext.IsTopLevelObject)
                {
                    // Silently fail if no files match. Will bind to an empty collection (treat empty as a success
                    // case and not reach here) if binding to a top-level object.
                    return;
                }

                // Perform any final type mangling needed.
                var modelType = bindingContext.ModelType;
                if (modelType == typeof(IRemoteStreamContent[]) || modelType == typeof(RemoteStreamContent[]))
                {
                    value = postedFiles.ToArray();
                }
                else
                {
                    value = postedFiles;
                }
            }

            // We need to add a ValidationState entry because the modelName might be non-standard. Otherwise
            // the entry we create in model state might not be marked as valid.
            bindingContext.ValidationState.Add(value, new ValidationStateEntry()
            {
                Key = modelName,
            });

            bindingContext.ModelState.SetModelValue(
                modelName,
                rawValue: null,
                attemptedValue: null);

            bindingContext.Result = ModelBindingResult.Success(value);
        }

        private async Task GetFormFilesAsync(
            string modelName,
            ModelBindingContext bindingContext,
            ICollection<IRemoteStreamContent> postedFiles)
        {
            var request = bindingContext.HttpContext.Request;
            if (request.HasFormContentType)
            {
                var form = await request.ReadFormAsync();

                foreach (var file in form.Files)
                {
                    // If there is an <input type="file" ... /> in the form and is left blank.
                    if (file.Length == 0 && string.IsNullOrEmpty(file.FileName))
                    {
                        continue;
                    }

                    if (file.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase))
                    {
                        postedFiles.Add(new RemoteStreamContent(file.OpenReadStream())
                        {
                            ContentType = file.ContentType,
                            FileName = file.FileName
                        });
                    }
                }
            }
        }
    }
}
