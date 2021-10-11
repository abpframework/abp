import {
  createLocalizationPipeKeyGenerator,
  createLocalizer,
  createLocalizerWithFallback,
  getLocaleDirection,
} from '../utils/localization-utils';

describe('Localization Utils', () => {
  describe('#getLocaleDirection', () => {
    test.each`
      locale       | expected
      ${undefined} | ${'ltr'}
      ${null}      | ${'ltr'}
      ${''}        | ${'ltr'}
      ${'en'}      | ${'ltr'}
      ${'en-US'}   | ${'ltr'}
      ${'pt'}      | ${'ltr'}
      ${'pt-PT'}   | ${'ltr'}
      ${'ar'}      | ${'rtl'}
      ${'ar-AE'}   | ${'rtl'}
      ${'ar-QA'}   | ${'rtl'}
      ${'ckb'}     | ${'rtl'}
      ${'ckb-IR'}  | ${'rtl'}
      ${'fa'}      | ${'rtl'}
      ${'fa-AF'}   | ${'rtl'}
      ${'he'}      | ${'rtl'}
      ${'ks'}      | ${'rtl'}
      ${'ksb'}     | ${'ltr'}
      ${'ksf'}     | ${'ltr'}
      ${'ksh'}     | ${'ltr'}
      ${'lrc'}     | ${'rtl'}
      ${'lrc-IQ'}  | ${'rtl'}
      ${'mzn'}     | ${'rtl'}
      ${'pa'}      | ${'ltr'}
      ${'pa-Arab'} | ${'rtl'}
      ${'ps'}      | ${'rtl'}
      ${'ps-PK'}   | ${'rtl'}
      ${'sd'}      | ${'rtl'}
      ${'ug'}      | ${'rtl'}
      ${'ur'}      | ${'rtl'}
      ${'ur-IN'}   | ${'rtl'}
      ${'uz'}      | ${'ltr'}
      ${'uz-Arab'} | ${'rtl'}
      ${'yi'}      | ${'rtl'}
      ${'zh'}      | ${'ltr'}
      ${'zh-Hans'} | ${'ltr'}
    `('should return $expected when $locale is given as parameter', ({ locale, expected }) => {
      expect(getLocaleDirection(locale)).toBe(expected);
    });
  });

  describe('#createLocalizer', () => {
    const localize = createLocalizer({
      values: { foo: { bar: 'baz' }, x: { y: 'z' } },
      defaultResourceName: 'x',
      currentCulture: null,
      languages: [],
      languageFilesMap: null,
      languagesMap: null,
    });

    test.each`
      resource     | key          | defaultValue | expected
      ${'_'}       | ${'TEST'}    | ${'DEFAULT'} | ${'TEST'}
      ${'foo'}     | ${'bar'}     | ${'DEFAULT'} | ${'baz'}
      ${'x'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'bar'}     | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${'y'}       | ${'DEFAULT'} | ${'z'}
      ${'a'}       | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${'y'}       | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${''}        | ${'DEFAULT'} | ${'DEFAULT'}
      ${'foo'}     | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'x'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${'a'}       | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${''}        | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
      ${undefined} | ${undefined} | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return $expected when resource name is $resource and key is $key',
      ({ resource, key, defaultValue, expected }) => {
        const result = localize(resource, key, defaultValue);

        expect(result).toBe(expected);
      },
    );
  });

  describe('#createLocalizerWithFallback', () => {
    const localizeWithFallback = createLocalizerWithFallback({
      values: { foo: { bar: 'baz' }, x: { y: 'z' } },
      defaultResourceName: 'x',
      currentCulture: null,
      languages: [],
      languageFilesMap: null,
      languagesMap: null,
    });

    test.each`
      resources          | keys                 | defaultValue | expected
      ${['', '_']}       | ${['TEST', 'OTHER']} | ${'DEFAULT'} | ${'TEST'}
      ${['foo']}         | ${['bar']}           | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['x']}           | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['y']}             | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['bar', 'y']}      | ${'DEFAULT'} | ${'baz'}
      ${['x']}           | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['a', 'b', 'c']} | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['']}            | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${[]}              | ${['bar', 'y']}      | ${'DEFAULT'} | ${'z'}
      ${['foo']}         | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return $expected when resource names are $resources and keys are $keys',
      ({ resources, keys, defaultValue, expected }) => {
        const result = localizeWithFallback(resources, keys, defaultValue);

        expect(result).toBe(expected);
      },
    );
  });

  describe('#createLocalizationPipeKeyGenerator', () => {
    const generateLocalizationPipeKey = createLocalizationPipeKeyGenerator({
      values: { foo: { bar: 'baz' }, x: { y: 'z' } },
      defaultResourceName: 'x',
      currentCulture: null,
      languages: [],
      languageFilesMap: null,
      languagesMap: null,
    });

    test.each`
      resources          | keys                 | defaultKey   | expected
      ${['', '_']}       | ${['TEST', 'OTHER']} | ${'DEFAULT'} | ${'TEST'}
      ${['foo']}         | ${['bar']}           | ${'DEFAULT'} | ${'foo::bar'}
      ${['x']}           | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['bar']}           | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${['y']}             | ${'DEFAULT'} | ${'x::y'}
      ${['x']}           | ${['y']}             | ${'DEFAULT'} | ${'x::y'}
      ${['a', 'b', 'c']} | ${['y']}             | ${'DEFAULT'} | ${'x::y'}
      ${['']}            | ${['y']}             | ${'DEFAULT'} | ${'x::y'}
      ${[]}              | ${['y']}             | ${'DEFAULT'} | ${'x::y'}
      ${['foo']}         | ${['bar', 'y']}      | ${'DEFAULT'} | ${'foo::bar'}
      ${['x']}           | ${['bar', 'y']}      | ${'DEFAULT'} | ${'x::y'}
      ${['a', 'b', 'c']} | ${['bar', 'y']}      | ${'DEFAULT'} | ${'x::y'}
      ${['']}            | ${['bar', 'y']}      | ${'DEFAULT'} | ${'x::y'}
      ${[]}              | ${['bar', 'y']}      | ${'DEFAULT'} | ${'x::y'}
      ${['foo']}         | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${['']}              | ${'DEFAULT'} | ${'DEFAULT'}
      ${['foo']}         | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['x']}           | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['a', 'b', 'c']} | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${['']}            | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
      ${[]}              | ${[]}                | ${'DEFAULT'} | ${'DEFAULT'}
    `(
      'should return $expected when resource names are $resources and keys are $keys',
      ({ resources, keys, defaultKey, expected }) => {
        const result = generateLocalizationPipeKey(resources, keys, defaultKey);

        expect(result).toBe(expected);
      },
    );
  });
});
