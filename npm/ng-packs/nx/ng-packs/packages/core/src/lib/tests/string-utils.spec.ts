import { createTokenParser, interpolate } from '../utils/string-utils';

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

  describe('#interpolate', () => {
    test.each`
      text                                         | params                          | expected
      ${'This is {0} and {1} example.'}            | ${['foo', 'bar']}               | ${'This is foo and bar example.'}
      ${'This is {1} and {0} example.'}            | ${['foo', 'bar']}               | ${'This is bar and foo example.'}
      ${'This is {0} and {0} example.'}            | ${['foo', 'bar']}               | ${'This is foo and foo example.'}
      ${'This is {1} and {1} example.'}            | ${['foo', 'bar']}               | ${'This is bar and bar example.'}
      ${'This is "{0}" and "{1}" example.'}        | ${['foo', 'bar']}               | ${'This is foo and bar example.'}
      ${"This is '{1}' and '{0}' example."}        | ${['foo', 'bar']}               | ${'This is bar and foo example.'}
      ${'This is { 0 } and {0} example.'}          | ${['foo', 'bar']}               | ${'This is foo and foo example.'}
      ${'This is {1} and {  1  } example.'}        | ${['foo', 'bar']}               | ${'This is bar and bar example.'}
      ${'This is {0}, {3}, {1}, and {2} example.'} | ${['foo', 'bar', 'baz', 'qux']} | ${'This is foo, qux, bar, and baz example.'}
      ${'This is {0} with 0 example.'}             | ${['foo']}                      | ${'This is foo with 0 example.'}
      ${'This is {0} and {1} example.'}            | ${['foo']}                      | ${'This is foo and {1} example.'}
      ${'This is {0} and {1} example.'}            | ${[]}                           | ${'This is {0} and {1} example.'}
      ${'This is {0} example.'}                    | ${[null]}                       | ${'This is {0} example.'}
    `(
      'should return $expected when text is $text and params are $params',
      ({ text, params, expected }) => {
        expect(interpolate(text, params)).toBe(expected);
      },
    );
  });
});
