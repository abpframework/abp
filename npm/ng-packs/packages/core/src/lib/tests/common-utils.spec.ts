import {
  isUndefinedOrEmptyString,
  noop,
  isNullOrUndefined,
  exists,
  isObject,
  isArray,
  isObjectAndNotArray,
} from '../utils';

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

  describe('#isNullOrUndefined & #exists', () => {
    test.each`
      value        | expected
      ${null}      | ${true}
      ${undefined} | ${true}
      ${true}      | ${false}
      ${false}     | ${false}
      ${''}        | ${false}
      ${'test'}    | ${false}
      ${0}         | ${false}
      ${10}        | ${false}
      ${[]}        | ${false}
      ${[1, 2]}    | ${false}
      ${{}}        | ${false}
      ${{ a: 1 }}  | ${false}
    `(
      'should return $expected and !$expected (for #exists) when given parameter is $value',
      ({ value, expected }) => {
        expect(isNullOrUndefined(value)).toBe(expected);
        expect(exists(value)).toBe(!expected);
      },
    );
  });

  describe('#isObject', () => {
    test.each`
      value        | expected
      ${null}      | ${false}
      ${undefined} | ${false}
      ${true}      | ${false}
      ${false}     | ${false}
      ${''}        | ${false}
      ${'test'}    | ${false}
      ${0}         | ${false}
      ${10}        | ${false}
      ${[]}        | ${true}
      ${[1, 2]}    | ${true}
      ${{}}        | ${true}
      ${{ a: 1 }}  | ${true}
    `('should return $expected when given parameter is $value', ({ value, expected }) => {
      expect(isObject(value)).toBe(expected);
    });
  });

  describe('#isArray', () => {
    test.each`
      value        | expected
      ${null}      | ${false}
      ${undefined} | ${false}
      ${true}      | ${false}
      ${false}     | ${false}
      ${''}        | ${false}
      ${'test'}    | ${false}
      ${0}         | ${false}
      ${10}        | ${false}
      ${[]}        | ${true}
      ${[1, 2]}    | ${true}
      ${{}}        | ${false}
      ${{ a: 1 }}  | ${false}
    `('should return $expected when given parameter is $value', ({ value, expected }) => {
      expect(isArray(value)).toBe(expected);
    });
  });

  describe('#isObjectAndNotArray', () => {
    test.each`
      value        | expected
      ${null}      | ${false}
      ${undefined} | ${false}
      ${true}      | ${false}
      ${false}     | ${false}
      ${''}        | ${false}
      ${'test'}    | ${false}
      ${0}         | ${false}
      ${10}        | ${false}
      ${[]}        | ${false}
      ${[1, 2]}    | ${false}
      ${{}}        | ${true}
      ${{ a: 1 }}  | ${true}
    `('should return $expected when given parameter is $value', ({ value, expected }) => {
      expect(isObjectAndNotArray(value)).toBe(expected);
    });
  });
});
