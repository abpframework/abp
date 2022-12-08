using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Pipeline.Receive;

namespace Volo.Abp.EventBus.Rebus;

public class AbpRebusEventHandlerStep : IIncomingStep
{
    public Task Process(IncomingStepContext context, Func<Task> next)
    {
        var message = context.Load<Message>();
        var handlerInvokers = context.Load<HandlerInvokers>().ToList();

        if (handlerInvokers.All(x => x.Handler is IRebusDistributedEventHandlerAdapter))
        {
            handlerInvokers = new List<HandlerInvoker> {handlerInvokers.Last()};
            context.Save(new HandlerInvokers(message, handlerInvokers));
        }
        
        return next();
    }
}
