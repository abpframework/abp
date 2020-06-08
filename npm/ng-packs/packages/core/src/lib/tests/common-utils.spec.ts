import { isUndefinedOrEmptyString, noop } from '../utils';

describe('CommonUtils', () => {
  describe('#noop', () => {
    test('should return empty fn', () => {
      expect(typeof noop()).toBe('function');
      expect(noop()()).toBeUndefined();
    });
  });

  describe('#isUndefinedOrEmptyString', () => {
    test.each`
      value        | expected
      ${null}      | ${false}
      ${0}         | ${false}
      ${true}      | ${false}
      ${'x'}       | ${false}
      ${{}}        | ${false}
      ${[]}        | ${false}
      ${undefined} | ${true}
      ${''}        | ${true}
    `('should return $expected when given parameter is $value', ({ value, expected }) => {
      expect(isUndefinedOrEmptyString(value)).toBe(expected);
    });
  });
});
