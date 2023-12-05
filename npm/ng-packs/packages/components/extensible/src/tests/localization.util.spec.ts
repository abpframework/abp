import { ApplicationLocalizationConfigurationDto } from '@abp/ng.core';
import { createDisplayNameLocalizationPipeKeyGenerator } from '../lib/utils/localization.util';

describe('Localization Utils', () => {
  describe('#createDisplayNameLocalizationPipeKeyGenerator', () => {
    const localization: ApplicationLocalizationConfigurationDto = {
      values:{
        Foo: { Bar: 'Bar', 'DisplayName:Bar': 'Bar' },
        Default: { Bar: 'Bar', 'DisplayName:Bar': 'Bar' },
      },
      defaultResourceName: 'Default',
      resources: {},
      languages: [],
      languageFilesMap: null,
      languagesMap: null,
      currentCulture: null
    }

    const generateDisplayName = createDisplayNameLocalizationPipeKeyGenerator(localization);

    test.each`
      displayName                         | fallback                                | expected
      ${{ name: 'Bar', resource: 'Foo' }} | ${null}                                 | ${'Foo::Bar'}
      ${{ name: 'Baz', resource: 'Foo' }} | ${null}                                 | ${'Baz'}
      ${null}                             | ${{ name: 'Bar', resource: 'Foo' }}     | ${'Foo::DisplayName:Bar'}
      ${null}                             | ${{ name: 'Bar', resource: 'Default' }} | ${'Default::DisplayName:Bar'}
      ${null}                             | ${{ name: 'Baz', resource: 'Default' }} | ${'Baz'}
    `(
      'should return $expected when diplay name is $displayName and fallback is $fallback',
      ({ displayName, fallback, expected }) => {
        const result = generateDisplayName(displayName, fallback);

        expect(result).toBe(expected);
      },
    );
  });
});
