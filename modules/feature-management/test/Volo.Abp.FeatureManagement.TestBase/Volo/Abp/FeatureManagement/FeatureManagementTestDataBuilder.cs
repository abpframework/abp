using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementTestDataBuilder : ITransientDependency
    {
        private readonly IFeatureValueRepository _featureValueRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly FeatureManagementTestData _testData;

        public FeatureManagementTestDataBuilder(
            IGuidGenerator guidGenerator,
            FeatureManagementTestData testData, 
            IFeatureValueRepository featureValueRepository)
        {
            _guidGenerator = guidGenerator;
            _testData = testData;
            _featureValueRepository = featureValueRepository;
        }

        public void Build()
        {
            // "Regular" edition features /////////////////////////////////////

            //SocialLogins
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.SocialLogins,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Regular
                )
            );

            //UserCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.UserCount,
                    "10",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Regular
                )
            );

            //ProjectCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.ProjectCount,
                    "1",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Regular
                )
            );

            // "Enterprise" edition features //////////////////////////////////

            //SocialLogins
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.SocialLogins,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //EmailSupport
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.EmailSupport,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //UserCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.UserCount,
                    "20",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //ProjectCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.ProjectCount,
                    "3",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //BackupCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.BackupCount,
                    "5",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            // "Ultimate" edition features ////////////////////////////////////

            //SocialLogins
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.SocialLogins,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //EmailSupport
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.EmailSupport,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //EmailSupport
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.DailyAnalysis,
                    true.ToString().ToLowerInvariant(),
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //UserCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.UserCount,
                    "100",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //ProjectCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.ProjectCount,
                    "10",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );

            //BackupCount
            _featureValueRepository.Insert(
                new FeatureValue(
                    _guidGenerator.Create(),
                    TestFeatureDefinitionProvider.BackupCount,
                    "10",
                    EditionFeatureManagementProvider.ProviderName,
                    TestEditionNames.Enterprise
                )
            );
        }
    }
}