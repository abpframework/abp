using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Settings
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IGuidGenerator _guidGenerator;

        public AbpIdentityTestDataBuilder(ISettingRepository settingRepository, IGuidGenerator guidGenerator)
        {
            _settingRepository = settingRepository;
            _guidGenerator = guidGenerator;
        }

        public void Build()
        {
            _settingRepository.InsertAsync(new Setting(_guidGenerator.Create(), "MySetting1", "42"));
            _settingRepository.InsertAsync(new Setting(_guidGenerator.Create(), "MySetting2", "55"));
        }
    }
}