using System;
using Volo.Abp;

namespace MyCompanyName.MyProjectName.Blazor.Pages
{
    public partial class Index
    {
        private void ThrowException()
        {
            throw new UserFriendlyException("Hey, there was a problem :(");
        }
    }
}
