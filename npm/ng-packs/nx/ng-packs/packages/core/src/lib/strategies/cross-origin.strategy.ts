export class CrossOriginStrategy {
  constructor(public crossorigin: 'anonymous' | 'use-credentials', public integrity?: string) {}

  setCrossOrigin<T extends HTMLElement>(element: T) {
    if (this.integrity) element.setAttribute('integrity', this.integrity);
    element.setAttribute('crossorigin', this.crossorigin);
  }
}

export class NoCrossOriginStrategy extends CrossOriginStrategy {
  setCrossOrigin() {}
}

export const CROSS_ORIGIN_STRATEGY = {
  Anonymous(integrity?: string) {
    return new CrossOriginStrategy('anonymous', integrity);
  },
  UseCredentials(integrity?: string) {
    return new CrossOriginStrategy('use-credentials', integrity);
  },
  None() {
    return new NoCrossOriginStrategy(null);
  },
};
