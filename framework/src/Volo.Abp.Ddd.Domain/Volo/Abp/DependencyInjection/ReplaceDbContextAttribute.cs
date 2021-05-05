using System;

namespace Volo.Abp.DependencyInjection
{
    public class ReplaceDbContextAttribute : Attribute
    {
        public Type[] ReplacedDbContextTypes { get; }

        public ReplaceDbContextAttribute(params Type[] replacedDbContextTypes)
        {
            ReplacedDbContextTypes = replacedDbContextTypes;
        }
    }
}
