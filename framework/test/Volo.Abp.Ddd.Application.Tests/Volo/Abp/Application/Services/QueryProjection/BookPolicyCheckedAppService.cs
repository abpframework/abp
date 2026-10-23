using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Application.Services.QueryProjection;

public class BookPolicyCheckedException : Exception
{

}

public class BookPolicyCheckedAppService : CrudAppService<Book, BookDto, Guid>
{
    public BookPolicyCheckedAppService(IRepository<Book, Guid> repository)
        : base(repository)
    {

    }

    protected override Task CheckGetPolicyAsync()
    {
        throw new BookPolicyCheckedException();
    }

    protected override Task CheckGetListPolicyAsync()
    {
        throw new BookPolicyCheckedException();
    }
}
