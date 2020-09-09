import { mapEnumToOptions } from '../utils/form-utils';

enum SomeEnum {
  NotApplicable = 'N/A',
  Foo = 0,
  Bar,
}

describe('Form Utils', () => {
  describe('#mapEnumToOptions', () => {
    it('should return options from enum', () => {
      const options = mapEnumToOptions(SomeEnum);

      expect(options).toEqual([
        {
          key: 'NotApplicable',
          value: SomeEnum.NotApplicable,
        },
        {
          key: 'Foo',
          value: SomeEnum.Foo,
        },
        {
          key: 'Bar',
          value: SomeEnum.Bar,
        },
      ]);
    });
  });
});
