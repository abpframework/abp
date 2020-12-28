using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Progression
{
    public interface IUiPageProgressService
    {
        Task Go(int? percentage, Action<UiPageProgressOptions> options = null);
    }
}
