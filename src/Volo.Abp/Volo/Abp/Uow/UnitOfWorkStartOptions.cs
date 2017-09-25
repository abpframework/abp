using System;
using System.Data;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkStartOptions
    {
        //Revise this since Begin/BeginReserved accepts different options and that can make confusion!

        public bool RequiresNew { get; set; }

        public string ReservationName { get; set; }

        public bool? IsTransactional { get; set; }

        public IsolationLevel? IsolationLevel { get; set; }

        public TimeSpan? Timeout { get; set; }
    }
}