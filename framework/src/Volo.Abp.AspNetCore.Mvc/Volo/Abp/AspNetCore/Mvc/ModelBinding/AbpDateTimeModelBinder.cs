using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.ModelBinding
{
    public class AbpDateTimeModelBinder : IModelBinder
    {
        private readonly DateTimeModelBinder _dateTimeModelBinder;
        private readonly IClock _clock;

        public AbpDateTimeModelBinder(IClock clock, DateTimeModelBinder dateTimeModelBinder)
        {
            _clock = clock;
            _dateTimeModelBinder = dateTimeModelBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            await _dateTimeModelBinder.BindModelAsync(bindingContext);
            if (bindingContext.Result.IsModelSet && bindingContext.Result.Model is DateTime dateTime)
            {
                bindingContext.Result = ModelBindingResult.Success(_clock.Normalize(dateTime));
            }
        }
    }
}
