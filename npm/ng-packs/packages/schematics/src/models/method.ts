import { eBindingSourceId, eMethodModifier } from '../enums';
import { camel } from '../utils';
import { ParameterInBody } from './api-definition';
import { Property } from './model';
import { Omissible } from './util';

export class Method {
  body: Body;
  signature: Signature;

  constructor(options: MethodOptions) {
    Object.assign(this, options);
  }
}

export type MethodOptions = Method;

export class Signature {
  generics = '';
  modifier = eMethodModifier.Public;
  name: string;
  parameters: Property[] = [];
  returnType = '';

  constructor(options: SignatureOptions) {
    Object.assign(this, options);
  }
}

export type SignatureOptions = Omissible<
  Signature,
  'generics' | 'modifier' | 'parameters' | 'returnType'
>;

export class Body {
  body?: string;
  method: string;
  params: string[] = [];
  requestType = 'any';
  responseType: string;
  url: string;

  registerActionParameter = (param: ParameterInBody) => {
    const { bindingSourceId, descriptorName, name, nameOnMethod } = param;
    const camelName = camel(name);
    const value = descriptorName ? `${descriptorName}.${camelName}` : nameOnMethod;

    switch (bindingSourceId) {
      case eBindingSourceId.Model:
      case eBindingSourceId.Query:
        this.params.push(`${camelName}: ${value}`);
        break;
      case eBindingSourceId.Body:
        this.body = value;
        break;
      case eBindingSourceId.Path:
        const regex = new RegExp('{(' + camelName + '|' + name + ')}', 'g');
        this.url = this.url.replace(regex, '${' + value + '}');
        break;
      default:
        break;
    }
  };

  constructor(options: BodyOptions) {
    Object.assign(this, options);
  }
}

export type BodyOptions = Omissible<
  Omit<Body, 'registerActionParameter'>,
  'params' | 'requestType'
>;
