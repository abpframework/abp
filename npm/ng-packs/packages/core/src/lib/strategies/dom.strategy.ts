export class DomStrategy {
  constructor(
    private getTarget: () => HTMLElement,
    public position: InsertPosition = 'beforeend',
  ) {}

  insertElement<T extends HTMLElement>(element: T) {
    if (typeof document !== 'undefined') {
      const target = this.getTarget();
      target.insertAdjacentElement(this.position, element);
    }
  }
}

export const DOM_STRATEGY = {
  AfterElement(element: HTMLElement) {
    return new DomStrategy(() => element, 'afterend');
  },
  AppendToBody() {
    return new DomStrategy(() => document?.body, 'beforeend');
  },
  AppendToHead() {
    return new DomStrategy(() => document?.head, 'beforeend');
  },
  BeforeElement(element: HTMLElement) {
    return new DomStrategy(() => element, 'beforebegin');
  },
  PrependToHead() {
    return new DomStrategy(() => document?.head, 'afterbegin');
  },
};
