using Microsoft.Extensions.Logging;

namespace Volo.Abp.DynamicProxy
{
	public class SimpleAsyncInterceptor2 : SimpleAsyncInterceptor
	{

        public SimpleAsyncInterceptor2(ILogger<AbpInterceptor> logger) : base(logger)
        {

        }
	}
}