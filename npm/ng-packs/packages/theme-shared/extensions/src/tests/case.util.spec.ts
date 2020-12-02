import { jsonNetCamelCase } from '../lib/utils/case.util';

describe('Case Utils', () => {
  describe('#jsonNetCamelCase', () => {
    test.each`
      input                 | output
      ${'Primary'}          | ${'primary'}
      ${'PrimaryRole'}      | ${'primaryRole'}
      ${'Primary Role'}     | ${'primary Role'}
      ${'PrimaryRole_Text'} | ${'primaryRole_Text'}
      ${'ISBN'}             | ${'isbn'}
      ${''}                 | ${''}
      ${'iMDB'}             | ${'iMDB'}
    `('should return $output when input is $input', ({ input, output }) => {
      expect(jsonNetCamelCase(input)).toBe(output);
    });
  });
});
