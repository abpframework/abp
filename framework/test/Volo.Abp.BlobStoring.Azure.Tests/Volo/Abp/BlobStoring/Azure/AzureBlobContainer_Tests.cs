using Xunit;
//#define I_HAVE_SET_THE_CORRECT_CONNECTIONSTRING_IN_THE_USERSECRETS_FILE

namespace Volo.Abp.BlobStoring.Azure
{
#if I_HAVE_SET_THE_CORRECT_CONNECTION_STRING_IN_THE_USERSECRETS_FILE
    public class AzureBlobContainer_Tests : BlobContainer_Tests<AbpBlobStoringAzureTestModule>
    {
        public AzureBlobContainer_Tests()
        {

        }
    }
#endif
}
