using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Uow;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> 
        where TDbContext : AbpDbContext<TDbContext>
    {
        private readonly TDbContext _dbContext;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkDbContextProvider(
            TDbContext dbContext, 
            IUnitOfWorkManager unitOfWorkManager) //TODO: Should create this dynamically inside a unit of work.
        {
            _dbContext = dbContext;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public TDbContext GetDbContext()
        {
            //if (_unitOfWorkManager.Current == null)
            //{
            //    throw new AbpException("A DbContext can only be created inside a unit of work!");
            //}

            return _dbContext;
        }
    }
}