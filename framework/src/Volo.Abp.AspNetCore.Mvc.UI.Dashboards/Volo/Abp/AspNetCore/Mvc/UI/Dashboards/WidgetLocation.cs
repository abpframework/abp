using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class WidgetLocation
    {
        public int X { get; set; }

        public int Y { get; set; }

        public WidgetLocation(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
