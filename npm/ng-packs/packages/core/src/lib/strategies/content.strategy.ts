import { ContentSecurityStrategy, CONTENT_SECURITY_STRATEGY } from './content-security.strategy';
import { DomStrategy, DOM_STRATEGY } from './dom.strategy';

export abstract class ContentStrategy<T extends HTMLScriptElement | HTMLStyleElement = any> {
  constructor(
    public content: string,
    protected domStrategy: DomStrategy = DOM_STRATEGY.AppendToHead(),
    protected contentSecurityStrategy: ContentSecurityStrategy = CONTENT_SECURITY_STRATEGY.None(),
  ) {}

  abstract createElement(): T;

  insertElement(): T {
    const element = this.createElement();

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
  AppendScriptToBody(content: string) {
    return new ScriptContentStrategy(content, DOM_STRATEGY.AppendToBody());
  },
  AppendScriptToHead(content: string) {
    return new ScriptContentStrategy(content, DOM_STRATEGY.AppendToHead());
  },
  AppendStyleToHead(content: string) {
    return new StyleContentStrategy(content, DOM_STRATEGY.AppendToHead());
  },
  PrependStyleToHead(content: string) {
    return new StyleContentStrategy(content, DOM_STRATEGY.PrependToHead());
  },
};
