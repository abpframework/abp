import { LocalizationService } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { PropData } from '../lib/models/props';
import { createEnum, createEnumOptions, createEnumValueResolver } from '../lib/utils/enum.util';

const fields = [
  { name: 'foo', value: 1 },
  { name: 'bar', value: 2 },
  { name: 'baz', value: 3 },
];

class MockPropData<R = any> extends PropData<R> {
  getInjected: PropData<R>['getInjected'];

  constructor(public readonly record: R) {
    super();
  }
}

describe('Enum Utils', () => {
  describe('#createEnum', () => {
    const enumFromFields = createEnum(fields);

    test.each`
      key      | expected
      ${'foo'} | ${1}
      ${'bar'} | ${2}
      ${'baz'} | ${3}
      ${1}     | ${'foo'}
      ${2}     | ${'bar'}
      ${3}     | ${'baz'}
    `('should create an enum that returns $expected when $key is accessed', ({ key, expected }) => {
      expect(enumFromFields[key]).toBe(expected);
    });
  });

  describe('#createEnumValueResolver', () => {
    const service = new LocalizationService(
      new Subject().asObservable(),
      ({
        selectSnapshot: () => ({
          values: {
            Default: {
              'Enum:MyEnum.foo': 'Foo',
              'MyEnum.bar': 'Bar',
              baz: 'Baz',
            },
          },
          defaultResourceName: 'Default',
          currentCulture: null,
          languages: [],
        }),
      } as unknown) as Store,
      null,
      null,
      null,
    );

    test.each`
      value | expected
      ${1}  | ${'Foo'}
      ${2}  | ${'Bar'}
      ${3}  | ${'Baz'}
    `(
      'should create a resolver that returns observable $expected when enum value is $value',
      async ({ value, expected }) => {
        const valueResolver = createEnumValueResolver(
          'MyCompanyName.MyProjectName.MyEnum',
          {
            fields,
            localizationResource: null,
            transformed: createEnum(fields),
          },
          'EnumProp',
        );
        const propData = new MockPropData({ extraProperties: { EnumProp: value } });
        propData.getInjected = () => service as any;

        const resolved = await valueResolver(propData)
          .pipe(take(1))
          .toPromise();

        expect(resolved).toBe(expected);
      },
    );
  });

  describe('#createEnumOptions', () => {
    const service = new LocalizationService(
      new Subject().asObservable(),
      ({
        selectSnapshot: () => ({
          values: {
            Default: {
              'Enum:MyEnum.foo': 'Foo',
              'MyEnum.bar': 'Bar',
              baz: 'Baz',
            },
          },
          defaultResourceName: 'Default',
          currentCulture: null,
          languages: [],
        }),
      } as unknown) as Store,
      null,
      null,
      null,
    );

    it('should create a generator that returns observable options from enums', async () => {
      const options = createEnumOptions('MyCompanyName.MyProjectName.MyEnum', {
        fields,
        localizationResource: null,
        transformed: createEnum(fields),
      });

      const propData = new MockPropData({});
      propData.getInjected = () => service as any;

      const resolved = await options(propData)
        .pipe(take(1))
        .toPromise();

      expect(resolved).toEqual([
        { key: 'Foo', value: 1 },
        { key: 'Bar', value: 2 },
        { key: 'Baz', value: 3 },
      ]);
    });
  });
});
