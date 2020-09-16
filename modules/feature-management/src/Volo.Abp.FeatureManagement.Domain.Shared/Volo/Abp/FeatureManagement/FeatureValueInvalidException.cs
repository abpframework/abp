using System;

namespace Volo.Abp.FeatureManagement
{
    [Serializable]
    public class FeatureValueInvalidException : BusinessException
    {
        public FeatureValueInvalidException(string name) :
            base(FeatureManagementDomainErrorCodes.FeatureValueInvalid)
        {
            WithData("0", name);
        }
    }
}
