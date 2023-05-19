import { CONTENT_SECURITY_STRATEGY, ContentSecurityStrategy } from './content-security.strategy';
import { DOM_STRATEGY, DomStrategy } from './dom.strategy';

export type ElementOptions<T extends HTMLScriptElement | HTMLStyleElement = any> = Partial<{
  [key in keyof T]: T[key];
}>;

export abstract class ContentStrategy<T extends HTMLScriptElement | HTMLStyleElement = any> {
  constructor(
    public content: string,
    protected domStrategy: DomStrategy = DOM_STRATEGY.AppendToHead(),
    protected contentSecurityStrategy: ContentSecurityStrategy = CONTENT_SECURITY_STRATEGY.None(),
    protected options: ElementOptions<T> = {},
  ) {}

  abstract createElement(): T;

  insertElement(): T {
    const element = this.createElement();

    if (this.options && Object.keys(this.options).length > 0) {
      (Object.keys(this.options) as Array<keyof ElementOptions<T>>).forEach(key => {
        if (this.options[key]) {
          element[key] = (this.options as NonNullable<T>)[key];
        }
      });
    }

    this.contentSecurityStrategy.applyCSP(element);
    this.domStrategy.insertElement(element);

    return element;
  }
}

export class StyleContentStrategy extends ContentStrategy<HTMLStyleElement> {
  createElement(): HTMLStyleElement {
    const element = document.createElement('style');
    element.textContent = this.content;

    return element;
  }
}

export class ScriptContentStrategy extends ContentStrategy<HTMLScriptElement> {
  createElement(): HTMLScriptElement {
    const element = document.createElement('script');
    element.textContent = this.content;

    return element;
  }
}

export const CONTENT_STRATEGY = {
  AppendScriptToBody(content: string, options?: ElementOptions<HTMLScriptElement>) {
    return new ScriptContentStrategy(content, DOM_STRATEGY.AppendToBody(), undefined, options);
  },
  AppendScriptToHead(content: string, options?: ElementOptions<HTMLScriptElement>) {
    return new ScriptContentStrategy(content, DOM_STRATEGY.AppendToHead(), undefined, options);
  },
  AppendStyleToHead(content: string, options?: ElementOptions<HTMLStyleElement>) {
    return new StyleContentStrategy(content, DOM_STRATEGY.AppendToHead(), undefined, options);
  },
  PrependStyleToHead(content: string, options?: ElementOptions<HTMLStyleElement>) {
    return new StyleContentStrategy(content, DOM_STRATEGY.PrependToHead(), undefined, options);
  },
};
