import { Import } from './import';
import { Options } from './util';

export class Model {
  readonly imports: Import[] = [];
  readonly interfaces: Interface[] = [];
  readonly namespace: string;
  readonly path: string;

  constructor(options: ModelOptions) {
    Object.assign(this, options);
  }
}

export type ModelOptions = Options<Model, 'imports' | 'interfaces'>;

export class Interface {
  readonly base: string | null;
  readonly identifier: string;
  readonly namespace: string;
  readonly generics: Generic[] = [];
  readonly properties: Property[] = [];
  readonly ref: string;

  constructor(options: InterfaceOptions) {
    Object.assign(this, options);
  }
}

export type InterfaceOptions = Options<Interface, 'generics' | 'properties'>;

abstract class TypeRef {
  readonly refs: string[] = [];

  protected _type = '';
  get type() {
    return this._type;
  }
  set type(value: string) {
    if (!value) return;
    this._type = value;
  }

  protected _default = '';
  get default() {
    return this._default;
  }
  set default(value: string) {
    if (!value) return;
    this._default = ` = ${value}`;
  }

  constructor(options: TypeRefOptions) {
    Object.assign(this, options);
  }

  setDefault(value: string) {
    this.default = value;
  }

  setType(value: string) {
    this.type = value;
  }
}

type TypeRefOptionalKeys = 'default' | 'refs';
type TypeRefOptions = Options<TypeRef, TypeRefOptionalKeys>;

export class Generic extends TypeRef {
  constructor(options: GenericOptions) {
    super(options);
  }
}

export type GenericOptions = Options<Generic, TypeRefOptionalKeys>;

export class Property extends TypeRef {
  readonly name: string;
  private _optional: '' | '?' = '';
  get optional() {
    return this.default ? '' : this._optional;
  }

  set optional(value: '' | '?') {
    this._optional = value;
  }

  constructor(options: PropertyOptions) {
    super(options);
  }

  setOptional(isOptional: boolean) {
    this.optional = isOptional ? '?' : '';
  }
}

export type PropertyOptions = Options<Property, TypeRefOptionalKeys | 'optional'>;
