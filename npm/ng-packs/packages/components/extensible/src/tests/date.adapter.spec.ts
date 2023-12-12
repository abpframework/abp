import { DateAdapter } from '../lib/adapters/date.adapter';

describe('Date Adapter', () => {
  const adapter = new DateAdapter();

  describe('#fromModel', () => {
    test.each`
      param           | expected
      ${undefined}    | ${null}
      ${null}         | ${null}
      ${'x'}          | ${null}
      ${'2002-03-30'} | ${{ day: 30, month: 3, year: 2002 }}
      ${'03/30/2002'} | ${{ day: 30, month: 3, year: 2002 }}
      ${new Date(0)}  | ${{ day: 1, month: 1, year: 1970 }}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.fromModel(param)).toEqual(expected);
    });
  });

  describe('#toModel', () => {
    test.each`
      param                                | expected
      ${undefined}                         | ${''}
      ${null}                              | ${''}
      ${{ day: 30, month: 3, year: 2002 }} | ${'2002-03-30'}
      ${{ day: 1, month: 1, year: 1970 }}  | ${'1970-01-01'}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.toModel(param)).toEqual(expected);
    });
  });
});
