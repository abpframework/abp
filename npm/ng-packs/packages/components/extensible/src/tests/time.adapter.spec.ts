import { TimeAdapter } from '../lib/adapters/time.adapter';

describe('Time Adapter', () => {
  const adapter = new TimeAdapter();

  describe('#fromModel', () => {
    const date = new Date();
    const hour = date.getHours();
    const minute = date.getMinutes();
    const second = date.getSeconds();

    test.each`
      param         | expected
      ${undefined}  | ${null}
      ${null}       | ${null}
      ${'x'}        | ${null}
      ${'13:30:45'} | ${{ hour: 13, minute: 30, second: 45 }}
      ${'13:30'}    | ${{ hour: 13, minute: 30, second: 0 }}
      ${date}       | ${{ hour, minute, second }}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.fromModel(param)).toEqual(expected);
    });
  });

  describe('#toModel', () => {
    test.each`
      param                                   | expected
      ${undefined}                            | ${''}
      ${null}                                 | ${''}
      ${{ hour: 13, minute: 30, second: 0 }}  | ${'13:30'}
      ${{ hour: 13, minute: 30, second: 45 }} | ${'13:30'}
    `('should return $expected when $param is given', ({ param, expected }) => {
      expect(adapter.toModel(param)).toEqual(expected);
    });
  });
});
