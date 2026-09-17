import { inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { DomStrategy } from '../strategies';

@Injectable({ providedIn: 'root' })
export class DomStrategyService {
  private document = inject(DOCUMENT);

  afterElement(el: HTMLElement) {
    return new DomStrategy(() => el, 'afterend');
  }

  beforeElement(el: HTMLElement) {
    return new DomStrategy(() => el, 'beforebegin');
  }

  appendToBody() {
    return new DomStrategy(() => this.document.body, 'beforeend');
  }

  appendToHead() {
    return new DomStrategy(() => this.document.head, 'beforeend');
  }

  prependToHead() {
    return new DomStrategy(() => this.document.head, 'afterbegin');
  }
}
