import { ABP } from '@abp/ng.core';
import { Injector, Type } from '@angular/core';
import { AsyncValidatorFn, ValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { O } from 'ts-toolbelt';
import {
  Prop,
  PropCallback,
  PropContributorCallback,
  PropContributorCallbacks,
  PropData,
  PropList,
  PropPredicate,
  Props,
  PropsFactory,
} from './props';

export class FormPropList<R = any> extends PropList<R, FormProp<R>> {}

export class FormProps<R = any> extends Props<FormPropList<R>> {
  protected _ctor: Type<FormPropList<R>> = FormPropList;
}

export class CreateFormPropsFactory<R = any> extends PropsFactory<FormProps<R>> {
  protected _ctor: Type<FormProps<R>> = FormProps;
}

export class EditFormPropsFactory<R = any> extends PropsFactory<FormProps<R>> {
  protected _ctor: Type<FormProps<R>> = FormProps;
}

export class FormProp<R = any> extends Prop<R> {
  readonly validators: PropCallback<R, ValidatorFn[]>;
  readonly asyncValidators: PropCallback<R, AsyncValidatorFn[]>;
  readonly disabled: PropPredicate<R>;
  readonly readonly: PropPredicate<R>;
  readonly autocomplete: string;
  readonly defaultValue: boolean | number | string | Date;
  readonly options: PropCallback<R, Observable<ABP.Option<any>[]>> | undefined;
  readonly id: string | undefined;

  constructor(options: FormPropOptions<R>) {
    super(
      options.type,
      options.name,
      options.displayName,
      options.permission,
      options.visible,
      options.isExtra,
    );

    this.asyncValidators = options.asyncValidators || (_ => []);
    this.validators = options.validators || (_ => []);
    this.disabled = options.disabled || (_ => false);
    this.readonly = options.readonly || (_ => false);
    this.autocomplete = options.autocomplete || 'off';
    this.options = options.options;
    this.id = options.id || options.name;
    const defaultValue = options.defaultValue;
    this.defaultValue = isFalsyValue(defaultValue) ? defaultValue : defaultValue || null;
  }

  static create<R = any>(options: FormPropOptions<R>) {
    return new FormProp<R>(options);
  }

  static createMany<R = any>(arrayOfOptions: FormPropOptions<R>[]) {
    return arrayOfOptions.map(FormProp.create);
  }
}

export class FormPropData<R = any> extends PropData<R> {
  getInjected: PropData<R>['getInjected'];

  constructor(injector: Injector, public readonly record: R) {
    super();

    this.getInjected = injector.get.bind(injector);
  }
}

export type FormPropOptions<R = any> = O.Optional<
  O.Writable<FormProp<R>>,
  | 'permission'
  | 'visible'
  | 'displayName'
  | 'isExtra'
  | 'validators'
  | 'asyncValidators'
  | 'disabled'
  | 'readonly'
  | 'autocomplete'
  | 'defaultValue'
  | 'options'
  | 'id'
>;

export type CreateFormPropDefaults<R = any> = Record<string, FormProp<R>[]>;
export type CreateFormPropContributorCallback<R = any> = PropContributorCallback<FormPropList<R>>;
export type CreateFormPropContributorCallbacks<R = any> = PropContributorCallbacks<FormPropList<R>>;
export type EditFormPropDefaults<R = any> = Record<string, FormProp<R>[]>;
export type EditFormPropContributorCallback<R = any> = PropContributorCallback<FormPropList<R>>;
export type EditFormPropContributorCallbacks<R = any> = PropContributorCallbacks<FormPropList<R>>;

function isFalsyValue(defaultValue: FormProp['defaultValue']): boolean {
  return [0, '', false].indexOf(defaultValue as any) > -1;
}
