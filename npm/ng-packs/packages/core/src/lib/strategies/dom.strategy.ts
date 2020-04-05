export class DomStrategy {
  constructor(
    public target: HTMLElement = document.head,
    public position: InsertPosition = 'beforeend',
  ) {}

  insertElement<T extends HTMLElement>(element: T) {
    this.target.insertAdjacentElement(this.position, element);
  }
}

export const DOM_STRATEGY = {
  AfterElement(element: HTMLElement) {
    return new DomStrategy(element, 'afterend');
  },
  AppendToBody() {
    return new DomStrategy(document.body, 'beforeend');
  },
  AppendToHead() {
    return new DomStrategy(document.head, 'beforeend');
  },
  BeforeElement(element: HTMLElement) {
    return new DomStrategy(element, 'beforebegin');
  },
  PrependToHead() {
    return new DomStrategy(document.head, 'afterbegin');
  },
};
