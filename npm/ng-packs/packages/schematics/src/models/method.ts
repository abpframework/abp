import { eBindingSourceId, eMethodModifier } from '../enums';
import { camel, camelizeHyphen } from '../utils/text';
import { getParamName } from '../utils/methods';
import { ParameterInBody } from './api-definition';
import { Property } from './model';
import { Omissible } from './util';
import {VOLO_REMOTE_STREAM_CONTENT} from "../constants";
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
  responseTypeWithNamespace: string;
  requestType = 'any';
  responseType: string;
  url: string;

  registerActionParameter = (param: ParameterInBody) => {
    const { bindingSourceId, descriptorName, jsonName, name, nameOnMethod } = param;
    const camelName = camel(name);
    const paramName = jsonName || camelName;
    let value = camelizeHyphen(nameOnMethod);
    if (descriptorName) {
      value = shouldQuote(paramName)
        ? `${descriptorName}['${paramName}']`
        : `${descriptorName}.${paramName}`;
    }

    switch (bindingSourceId) {
      case eBindingSourceId.Model:
      case eBindingSourceId.Query:
        this.params.push(paramName === value ? value : `${getParamName(paramName)}: ${value}`);
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

  isBlobMethod(){
    return this.responseTypeWithNamespace === VOLO_REMOTE_STREAM_CONTENT
  }

  private setUrlQuotes() {
    this.url = /{/.test(this.url) ? `\`/${this.url}\`` : `'/${this.url}'`;
  }
}

export type BodyOptions = Omissible<
  Omit<Body, 'registerActionParameter'>,
  'params' | 'requestType' | 'isBlobMethod'
>;
