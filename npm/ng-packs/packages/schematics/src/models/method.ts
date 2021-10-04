import { eBindingSourceId, eMethodModifier } from '../enums';
import { camel } from '../utils/text';
import { ParameterInBody } from './api-definition';
import { Property } from './model';
import { Omissible } from './util';
// eslint-disable-next-line @typescript-eslint/no-var-requires
const shouldQuote = require('should-quote');

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
    const { bindingSourceId, descriptorName, jsonName, name, nameOnMethod } = param;
    const camelName = camel(name);
    const paramName = jsonName || camelName;
    const value = descriptorName
      ? shouldQuote(paramName)
        ? `${descriptorName}['${paramName}']`
        : `${descriptorName}.${paramName}`
      : nameOnMethod;

    switch (bindingSourceId) {
      case eBindingSourceId.Model:
      case eBindingSourceId.Query:
        this.params.push(paramName === value ? value : `${paramName}: ${value}`);
        break;
      case eBindingSourceId.Body:
        this.body = value;
        break;
      case eBindingSourceId.Path:
        // eslint-disable-next-line no-case-declarations
        const regex = new RegExp('{(' + paramName + '|' + camelName + '|' + name + ')}', 'g');
        this.url = this.url.replace(regex, '${' + value + '}');
        break;
      default:
        break;
    }
  };

  constructor(options: BodyOptions) {
    Object.assign(this, options);
    this.setUrlQuotes();
  }

  private setUrlQuotes() {
    this.url = /{/.test(this.url) ? `\`/${this.url}\`` : `'/${this.url}'`;
  }
}

export type BodyOptions = Omissible<
  Omit<Body, 'registerActionParameter'>,
  'params' | 'requestType'
>;
