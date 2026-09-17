import {
  findMatchingCultureName,
  getFirstPathSegment,
  normalizeUrlForRouteCultureMatch,
  stripCultureSegmentFromPath,
} from '../utils/route-based-culture.utils';
import type { LanguageInfo } from '../proxy/volo/abp/localization/models';

describe('route-based-culture.utils', () => {
  describe('#getFirstPathSegment', () => {
    test.each`
      urlPath                      | expected
      ${''}                        | ${''}
      ${'/'}                       | ${''}
      ${'/en'}                     | ${'en'}
      ${'/en/account'}             | ${'en'}
      ${'en/account'}              | ${'en'}
      ${'/tr-TR/foo/bar'}          | ${'tr-TR'}
      ${'/zh-Hans/account?x=1'}    | ${'zh-Hans'}
      ${'tr/home#frag'}            | ${'tr'}
    `('should return $expected when urlPath is $urlPath', ({ urlPath, expected }) => {
      expect(getFirstPathSegment(urlPath)).toBe(expected);
    });
  });

  describe('#findMatchingCultureName', () => {
    const languages: LanguageInfo[] = [
      { cultureName: 'en' },
      { cultureName: 'tr-TR' },
      { cultureName: 'zh-Hans' },
    ];

    test.each`
      segment      | expected
      ${'en'}      | ${'en'}
      ${'EN'}      | ${'en'}
      ${'tr-tr'}   | ${'tr-TR'}
      ${'zh-hans'} | ${'zh-Hans'}
      ${'account'} | ${undefined}
      ${''}        | ${undefined}
    `('should return $expected when segment is $segment', ({ segment, expected }) => {
      expect(findMatchingCultureName(segment, languages)).toBe(expected);
    });

    test('should return undefined when languages is empty', () => {
      expect(findMatchingCultureName('en', [])).toBeUndefined();
    });
  });

  describe('#stripCultureSegmentFromPath', () => {
    const languages: LanguageInfo[] = [{ cultureName: 'en' }, { cultureName: 'tr' }];

    test('should strip leading culture segment for menu matching', () => {
      expect(stripCultureSegmentFromPath('/en/identity/users', languages)).toBe('/identity/users');
    });

    test('should leave path unchanged when first segment is not a culture', () => {
      expect(stripCultureSegmentFromPath('/identity/users', languages)).toBe('/identity/users');
    });

    test('should preserve query string', () => {
      expect(stripCultureSegmentFromPath('/en/home?x=1', languages)).toBe('/home?x=1');
    });
  });

  describe('#normalizeUrlForRouteCultureMatch', () => {
    const languages: LanguageInfo[] = [{ cultureName: 'en' }];

    test('should no-op when useRouteBasedCulture is false', () => {
      expect(normalizeUrlForRouteCultureMatch('/en/identity/users', false, languages)).toBe(
        '/en/identity/users',
      );
    });

    test('should strip when enabled', () => {
      expect(normalizeUrlForRouteCultureMatch('/en/identity/users', true, languages)).toBe(
        '/identity/users',
      );
    });
  });
});
