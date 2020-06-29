import { DateTimeAdapter } from '../lib/adapters/date-time.adapter';

describe('DateTime Adapter', () => {
  const adapter = new DateTimeAdapter();
  const date = new Date(2002, 2, 30, 13, 30, 45, 0);
  const year = date.getFullYear();
  const month = date.getMonth() + 1;
  const day = date.getDate();
  const hour = date.getHours();
  const minute = date.getMinutes();
  const second = date.getSeconds();

  describe('#fromModel', () => {
    test.each`
      param        | expected
      ${undefined} | ${null}
      ${null}      | ${null}
      ${'x'}       | ${null}
      ${date}      | ${{ year, month, day, hour, minute, second }}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.fromModel(param)).toEqual(expected);
    });
  });

  describe('#toModel', () => {
    test.each`
      param                                         | expected
      ${undefined}                                  | ${''}
      ${null}                                       | ${''}
      ${{ year, month, day, hour, minute, second }} | ${date.toISOString()}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.toModel(param)).toEqual(expected);
    });
  });
});
