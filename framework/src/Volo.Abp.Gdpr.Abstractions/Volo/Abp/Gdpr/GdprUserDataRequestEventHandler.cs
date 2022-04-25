using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.Gdpr;

public class GdprUserDataRequestEventHandler
    : IDistributedEventHandler<GdprUserDataRequestedEto>, ITransientDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IDistributedEventBus EventBus { get; }
    
    public GdprUserDataRequestEventHandler(IServiceScopeFactory serviceScopeFactory, IDistributedEventBus eventBus)
    {
        ServiceScopeFactory = serviceScopeFactory;
        EventBus = eventBus;
    }
    
    public async Task HandleEventAsync(GdprUserDataRequestedEto eventData)
    {
        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var gdprDataProviders = scope.ServiceProvider.GetServices<IGdprUserDataProvider>().ToList();
            
            foreach (var gdprDataProvider in gdprDataProviders)
            {
                var gdprDataInfo = await gdprDataProvider.GetAsync(new GdprUserDataProviderContext { UserId = eventData.UserId});
                
                await EventBus.PublishAsync(
                    new GdprUserDataPreparedEto
                    {
                        RequestId = eventData.RequestId, 
                        Data = gdprDataInfo,
                        Provider = gdprDataProvider.GetType().FullName
                    });
            }
        }
    }
}