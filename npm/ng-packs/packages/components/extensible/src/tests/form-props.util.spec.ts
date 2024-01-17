import { LocalizationService } from '@abp/ng.core';
import { Injector } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { ePropType } from '../lib/enums/props.enum';
import { FormProp, FormPropData } from '../lib/models/form-props';
import { ExtensionsService } from '../lib/services/extensions.service';
import { EXTENSIONS_IDENTIFIER } from '../lib/tokens/extensions.token';
import { generateFormFromProps } from '../lib/utils/form-props.util';

describe('Form Prop Utils', () => {
  describe('#generateFormFromProps', () => {
    let spectator: SpectatorService<ExtensionsService<Foo>>;
    let injector: Injector;
    const identifier = 'X';

    const createService = createServiceFactory({
      service: ExtensionsService,
      providers: [
        {
          provide: EXTENSIONS_IDENTIFIER,
          useValue: identifier,
        },
        {
          provide: LocalizationService,
          useValue: { currentLang: 'en' },
        },
      ],
    });

    beforeEach(() => {
      spectator = createService();
      const props = FormProp.createMany<Foo>([
        {
          type: ePropType.String,
          name: 'foo',
          validators: () => [Validators.required],
          defaultValue: 'bar',
        },
        {
          type: ePropType.Boolean,
          name: 'bool',
        },
        {
          type: ePropType.Date,
          name: 'date',
        },
        {
          type: ePropType.DateTime,
          name: 'dateTime',
        },
        {
          type: ePropType.Time,
          name: 'time',
        },
      ]);

      spectator.service.createFormProps
        .get(identifier)
        .addContributor(propList => propList.addManyTail(props));
      spectator.service.editFormProps
        .get(identifier)
        .addContributor(propList => propList.addManyTail(props));

      const generator = getInjected(spectator);
      injector = {
        get: () => generator.next().value as any,
      };
    });

    it('should return a blank FormGroup instance', () => {
      const data = new FormPropData<Foo>(injector, null);

      const formGroup = generateFormFromProps(data);
      expect(formGroup).toBeInstanceOf(FormGroup);
      expect(formGroup.value.foo).toBe('bar');

      const formControl = formGroup.get('foo');
      expect(formControl).toBeInstanceOf(FormControl);
      expect(formControl.valid).toBe(true);
    });

    it('should return a prefilled FormGroup instance', () => {
      const data = new FormPropData<Foo>(injector, { id: 1, foo: null });

      const formGroup = generateFormFromProps(data);
      expect(formGroup).toBeInstanceOf(FormGroup);
      expect(formGroup.value.foo).toBe(null);

      const formControl = formGroup.get('foo');
      expect(formControl).toBeInstanceOf(FormControl);
      expect(formControl.invalid).toBe(true);
    });

    it('should add a FormGroup named extraProperties', () => {
      const data = new FormPropData<Foo>(injector, null);

      const formGroup = generateFormFromProps(data);
      const extraPropertiesGroup = formGroup.get('extraProperties');
      expect(extraPropertiesGroup).toBeInstanceOf(FormGroup);
    });

    it('should add extraProperties to extraProperties FormGroup', () => {
      const data = new FormPropData<Foo>(injector, {
        id: 1,
        foo: undefined,
        extraProperties: {
          bool: true,
          date: '03/30/2002',
          dateTime: '2002-03-30 13:30:59Z',
          time: '13:30:59',
        },
      });

      const formGroup = generateFormFromProps(data);
      const extraPropertiesGroup = formGroup.get('extraProperties');
      expect(extraPropertiesGroup.value).toEqual({
        bool: true,
        date: '2002-03-30',
        dateTime: '2002-03-30T13:30:59.000Z',
        time: '13:30',
      });
    });
  });
});

function* getInjected(spectator: SpectatorService<ExtensionsService>) {
  yield spectator.service;
  yield spectator.inject(EXTENSIONS_IDENTIFIER);
  yield spectator.inject(LocalizationService);
}

interface Foo {
  id: number;
  foo: string;
  extraProperties?: {
    bool: boolean;
    date: string;
    dateTime: string;
    time: string;
  };
}
