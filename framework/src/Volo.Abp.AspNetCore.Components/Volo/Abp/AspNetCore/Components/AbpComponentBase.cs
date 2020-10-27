using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.AspNetCore.Components
{
    public class AbpComponentBase : OwningComponentBase
    {
        //TODO: IStringLocalizer, CurrentUser, Logger, AuthorizationService

        protected IObjectMapper ObjectMapper
        {
            get
            {
                if (_objectMapper != null)
                {
                    return _objectMapper;
                }

                if (ObjectMapperContext == null)
                {
                    return LazyGetRequiredService(ref _objectMapper);
                }

                return LazyGetRequiredService(
                    typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext),
                    ref _objectMapper
                );
            }
        }

        private IObjectMapper _objectMapper;
        protected Type ObjectMapperContext { get; set; }

        protected TService LazyGetRequiredService<TService>(ref TService reference) => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                reference = (TRef)ScopedServices.GetRequiredService(serviceType);
            }

            return reference;
        }
    }
}
