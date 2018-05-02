using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayOrder : Attribute
    {
        public static int Default = 10000;

        public int Number { get; set; }

        public DisplayOrder(int number)
        {
            Number = number;
        }
    }
}
