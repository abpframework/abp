import { deepMerge } from '../utils/object-utils';

describe('DeepMerge', () => {
  test.each`
    target       | source
    ${null}      | ${null}
    ${null}      | ${undefined}
    ${undefined} | ${null}
    ${undefined} | ${undefined}
  `('should return empty object when both inputs are $target and $source', ({ target, source }) => {
    expect(deepMerge(target, source)).toEqual({});
  });

  test.each`
    value
    ${10}
    ${false}
    ${''}
    ${'test-string'}
    ${{ a: 1 }}
    ${[1, 2, 3]}
    ${{}}
  `('should correctly return when any of the inputs is null or undefined', val => {
    expect(deepMerge(undefined, val)).toEqual(val);
    expect(deepMerge(null, val)).toEqual(val);
    expect(deepMerge(val, undefined)).toEqual(val);
    expect(deepMerge(val, null)).toEqual(val);
  });

  test.each`
    target           | source
    ${10}            | ${false}
    ${false}         | ${20}
    ${'some-string'} | ${{ a: 5 }}
    ${{ b: 10 }}     | ${50}
    ${[1, 2, 3]}     | ${40}
    ${{ k: 60 }}     | ${[4, 5, 6]}
  `(
    'should correctly return source if one of them is primitive or an array',
    ({ target, source }) => {
      expect(deepMerge(target, source)).toEqual(source);
    },
  );

  it('should correctly return when both inputs are objects with different fields', () => {
    const target = { a: 1 };
    const source = { b: 2 };
    const expected = { a: 1, b: 2 };
    expect(deepMerge(target, source)).toEqual(expected);
    expect(deepMerge(source, target)).toEqual(expected);
  });

  it('should correctly return when both inputs are object with same fields but different values', () => {
    const target = { a: 1 };
    const source = { a: 5 };
    expect(deepMerge(target, source)).toEqual(source);
    expect(deepMerge(source, target)).toEqual(target);
  });

  it('should correctly merge shallow objects with different fields as well as some shared ones', () => {
    const target = { a: 1, b: 2, c: 3 };
    const source = { a: 4, d: 5, e: 6 };
    expect(deepMerge(target, source)).toEqual({ a: 4, b: 2, c: 3, d: 5, e: 6 });
  });

  it('should not merge arrays and return the latter', () => {
    const firstArray = [1, 2, 3];
    const secondArray = [3, 4, 5, 6];
    expect(deepMerge(firstArray, secondArray)).toEqual(secondArray);
    const target = { a: firstArray };
    const source = { a: secondArray };
    expect(deepMerge(target, source)).toEqual({ a: secondArray });
  });

  it('should correctly merge nested objects', () => {
    const target = {
      a: {
        b: {
          c: {
            d: 1,
            g: 10,
            q: undefined,
            t: false,
          },
          e: {
            f: [1, 2, 3],
            p: 'other-string',
          },
        },
      },
      x: {
        q: 'some-string',
      },
    };
    const source = {
      a: {
        b: {
          c: {
            h: 30,
            q: 45,
            t: null,
          },
          m: 20,
          e: {
            f: [20, 30, 40],
          },
        },
        e: { k: [5, 6] },
      },
      z: {
        y: true,
      },
    };
    const expected = {
      a: {
        b: {
          c: {
            d: 1,
            g: 10,
            h: 30,
            q: 45,
            t: false,
          },
          e: {
            f: [20, 30, 40],
            p: 'other-string',
          },
          m: 20,
        },
        e: { k: [5, 6] },
      },
      x: {
        q: 'some-string',
      },
      z: {
        y: true,
      },
    };
    expect(deepMerge(target, source)).toEqual(expected);
  });
});
