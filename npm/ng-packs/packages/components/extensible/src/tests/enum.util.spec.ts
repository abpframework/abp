import { ConfigStateService, ExtensionEnumFieldDto, LocalizationService } from '@abp/ng.core';
import { BehaviorSubject, of } from 'rxjs';
import { take } from 'rxjs/operators';
import { PropData } from '../lib/models/props';
import { createEnum, createEnumOptions, createEnumValueResolver } from '../lib/utils/enum.util';
import { TestBed } from '@angular/core/testing';

const mockSessionState = {
  languageChange$: new BehaviorSubject('tr'),
  getLanguage: () => 'tr',
  getLanguage$: () => of('tr'),
  onLanguageChange$: () => new BehaviorSubject('tr'),
} as any;

const fields: ExtensionEnumFieldDto[] = [
  { name: 'foo', value: { number: 1 } },
  { name: 'bar', value: { number: 2 } },
  { name: 'baz', value: { number: 3 } },
];

class MockPropData<R = any> extends PropData<R> {
  getInjected: PropData<R>['getInjected'];

  constructor(public readonly record: R) {
    super();
  }
}

const mockL10n = {
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
};

describe('Enum Utils', () => {
  let configStateService: ConfigStateService;
  let localizationService: LocalizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: ConfigStateService,
          useValue: {
            refreshAppState: jest.fn(),
            getAll: jest.fn(),
            getOne: jest.fn(),
            getOne$: jest.fn(),
            getDeep: jest.fn(),
            getDeep$: jest.fn(),
          },
        },
        {
          provide: LocalizationService,
          useValue: {
            get: jest.fn(),
            instant: jest.fn(),
            localizeWithFallbackSync: jest.fn().mockImplementation((resource, keys, defaultValue) => {
              if (keys.includes('Enum:MyEnum.foo')) return 'Foo';
              if (keys.includes('MyEnum.bar')) return 'Bar';
              if (keys.includes('MyEnum.baz')) return 'Baz';
              if (keys.includes('baz')) return 'Baz';
              if (keys.includes('foo')) return 'Foo';
              return defaultValue;
            }),
          },
        },
      ],
    });

    configStateService = TestBed.inject(ConfigStateService);
    localizationService = TestBed.inject(LocalizationService);
  });

  describe('#createEnum', () => {
    const enumFromFields = createEnum(fields);

    test.each([
      { name: 'foo', value: 'number', expected: 1 },
      { name: 'bar', value: 'number', expected: 2 },
      { name: 'baz', value: 'number', expected: 3 },
    ])('should create an enum that returns $expected when $name $value is accessed', ({ name, value, expected }) => {
      expect(enumFromFields[name][value]).toBe(expected);
    });
  });

  describe('#createEnumValueResolver', () => {
    test.each`
      value | expected
      ${{ number: 3 }}  | ${'Baz'}
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
        const propData = new MockPropData({
          extraProperties: { EnumProp: value },
        });
        propData.getInjected = () => localizationService as any;

        const resolved = await valueResolver(propData).pipe(take(1)).toPromise();

        expect(resolved).toBe(expected);
      },
    );
  });

  describe('#createEnumOptions', () => {
    it('should create a generator that returns observable options from enums', async () => {
      const options = createEnumOptions('MyCompanyName.MyProjectName.MyEnum', {
        fields,
        localizationResource: null,
        transformed: createEnum(fields),
      });

      const propData = new MockPropData({});
      propData.getInjected = () => localizationService as any;

      const resolved = await options(propData).pipe(take(1)).toPromise();

      expect(resolved).toEqual([
        { key: 'Foo', value: { number: 1 } },
        { key: 'Bar', value: { number: 2 } },
        { key: 'Baz', value: { number: 3 } },
      ]);
    });
  });
});
