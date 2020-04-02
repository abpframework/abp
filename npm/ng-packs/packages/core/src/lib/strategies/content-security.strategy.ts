export abstract class ContentSecurityStrategy {
  constructor(public nonce?: string) {}

  abstract applyCSP(element: HTMLScriptElement | HTMLStyleElement): void;
}

export class StrictContentSecurityStrategy extends ContentSecurityStrategy {
  constructor(nonce: string) {
    super(nonce);
  }

  applyCSP(element: HTMLScriptElement | HTMLStyleElement) {
    element.setAttribute('nonce', this.nonce);
  }
}

export class LooseContentSecurityStrategy extends ContentSecurityStrategy {
  constructor() {
    super();
  }

  applyCSP(_: HTMLScriptElement | HTMLStyleElement) {}
}

export const CONTENT_SECURITY_STRATEGY = {
  Loose() {
    return new LooseContentSecurityStrategy();
  },
  Strict(nonce: string) {
    return new StrictContentSecurityStrategy(nonce);
  },
};
