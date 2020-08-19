import { Import } from './import';
import { Omissible } from './util';

export class Model {
  imports: Import[] = [];
  interfaces: Interface[] = [];
  namespace: string;

  constructor(options: ModelOptions) {
    Object.assign(this, options);
  }
}

export type ModelOptions = Omissible<Model, 'imports' | 'interfaces'>;

export class Interface {
  base: string | null;
  identifier: string;
  properties: Property[] = [];

  constructor(options: InterfaceOptions) {
    Object.assign(this, options);
  }
}

export type InterfaceOptions = Omissible<Interface, 'properties'>;

export class Property {
  name: string;
  type: string;
  default: string = '';
  optional: '' | '?' = '';

  constructor(options: PropertyOptions) {
    Object.assign(this, options);
  }
}

export type PropertyOptions = Omissible<Property, 'default' | 'optional'>;
