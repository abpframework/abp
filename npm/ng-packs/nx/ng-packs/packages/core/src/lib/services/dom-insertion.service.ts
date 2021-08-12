import { Injectable } from '@angular/core';
import { ContentStrategy } from '../strategies/content.strategy';
import { generateHash } from '../utils';

@Injectable({ providedIn: 'root' })
export class DomInsertionService {
  private readonly inserted = new Set<number>();

  insertContent<T extends HTMLScriptElement | HTMLStyleElement>(
    contentStrategy: ContentStrategy<T>,
  ): T {
    const hash = generateHash(contentStrategy.content);

    if (this.inserted.has(hash)) return;

    const element = contentStrategy.insertElement();
    this.inserted.add(hash);

    return element;
  }

  removeContent(element: HTMLScriptElement | HTMLStyleElement) {
    const hash = generateHash(element.textContent);
    this.inserted.delete(hash);

    element.parentNode.removeChild(element);
  }

  has(content: string): boolean {
    const hash = generateHash(content);

    return this.inserted.has(hash);
  }
}
