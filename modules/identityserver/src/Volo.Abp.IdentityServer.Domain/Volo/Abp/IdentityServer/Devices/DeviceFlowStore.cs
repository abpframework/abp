using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.IdentityServer.Devices
{
    public class DeviceFlowStore : IDeviceFlowStore, ITransientDependency
    {
        protected IDeviceFlowCodesRepository DeviceFlowCodesRepository { get; }
        protected IGuidGenerator GuidGenerator { get; }
        protected IPersistentGrantSerializer PersistentGrantSerializer { get; }

        public DeviceFlowStore(
            IDeviceFlowCodesRepository deviceFlowCodesRepository, 
            IGuidGenerator guidGenerator, 
            IPersistentGrantSerializer persistentGrantSerializer)
        {
            DeviceFlowCodesRepository = deviceFlowCodesRepository;
            GuidGenerator = guidGenerator;
            PersistentGrantSerializer = persistentGrantSerializer;
        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            Check.NotNull(deviceCode, nameof(deviceCode));
            Check.NotNull(userCode, nameof(userCode));
            Check.NotNull(data, nameof(data));

            await DeviceFlowCodesRepository.InsertAsync(
                new DeviceFlowCodes(GuidGenerator.Create())
                {
                    DeviceCode = deviceCode,
                    UserCode = userCode,
                    ClientId = data.ClientId,
                    SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                    CreationTime = data.CreationTime,
                    Expiration = data.CreationTime.AddSeconds(data.Lifetime),
                    Data = Serialize(data)
                }
            ).ConfigureAwait(false);
        }
        
        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            Check.NotNull(userCode, nameof(userCode));

            var deviceCodes = await DeviceFlowCodesRepository.FindByUserCodeAsync(userCode).ConfigureAwait(false);
            if (deviceCodes == null)
            {
                return null;
            }

            return DeserializeToDeviceCode(deviceCodes.Data);
        }

        public Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            throw new NotImplementedException();
        }

        public Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            throw new NotImplementedException();
        }

        public Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            throw new NotImplementedException();
        }

        private string Serialize([CanBeNull] DeviceCode deviceCode)
        {
            if (deviceCode == null)
            {
                return null;
            }

            return PersistentGrantSerializer.Serialize(deviceCode);
        }

        protected virtual DeviceCode DeserializeToDeviceCode([CanBeNull] string data)
        {
            if (data == null)
            {
                return null;
            }

            return PersistentGrantSerializer.Deserialize<DeviceCode>(data);
        }
    }
}
