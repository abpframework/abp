import { createTokenParser } from '../utils/string-utils';

describe('String Utils', () => {
  describe('#createTokenParser', () => {
    const parseTokens = createTokenParser('{subDomain}.{domain}.{gtld}|{domain}.{gtld}');

    test.each`
      url                       | subDomain     | domain       | gtld
      ${'www.example.com'}      | ${'www'}      | ${'example'} | ${'com'}
      ${'test.sub.example.com'} | ${'test.sub'} | ${'example'} | ${'com'}
      ${'example.com'}          | ${undefined}  | ${'example'} | ${'com'}
    `(
      'should return subDomain as $subDomain, domain as $domain, and gtld as $gtld when url is $url',
      ({ url, subDomain, domain, gtld }) => {
        const parsed = parseTokens(url);
        expect(parsed.subDomain[0]).toBe(subDomain);
        expect(parsed.domain[0]).toBe(domain);
        expect(parsed.gtld[0]).toBe(gtld);
      },
    );
  });
});
