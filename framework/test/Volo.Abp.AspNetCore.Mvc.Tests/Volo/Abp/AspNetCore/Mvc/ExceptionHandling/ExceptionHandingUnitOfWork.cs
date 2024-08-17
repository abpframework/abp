using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.ExceptionHandling;

public class ExceptionHandingUnitOfWork : UnitOfWork
{
    public ExceptionHandingUnitOfWork(
        IServiceProvider serviceProvider,
        IUnitOfWorkEventPublisher unitOfWorkEventPublisher,
        IOptions<AbpUnitOfWorkDefaultOptions> options)
        : base(serviceProvider, unitOfWorkEventPublisher, options)
    {

    }
    public async override Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (ServiceProvider.GetRequiredService<ICurrentUser>().Id == Guid.Empty)
        {
            throw new AbpDbConcurrencyException();
        }

        await base.SaveChangesAsync(cancellationToken);
    }
}
