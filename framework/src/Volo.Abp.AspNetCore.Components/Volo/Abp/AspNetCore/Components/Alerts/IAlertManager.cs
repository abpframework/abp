using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.Alerts;

public interface IAlertManager
{
    AlertList Alerts { get; }
}
