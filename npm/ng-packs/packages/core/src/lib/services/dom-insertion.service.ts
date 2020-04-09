import { Injectable, Injector, TemplateRef, Type } from '@angular/core';
import { ContentStrategy } from '../strategies/content.strategy';
import { ProjectionStrategy } from '../strategies/projection.strategy';
import { generateHash } from '../utils';

@Injectable({ providedIn: 'root' })
export class DomInsertionService {
  readonly inserted = new Set<number>();

  constructor(private injector: Injector) {}

  insertContent(contentStrategy: ContentStrategy) {
    const hash = generateHash(contentStrategy.content);

    if (this.inserted.has(hash)) return;

    contentStrategy.insertElement();
    this.inserted.add(hash);
  }

  projectContent<T extends Type<any> | TemplateRef<any>>(
    projectionStrategy: ProjectionStrategy<T>,
    injector = this.injector,
  ) {
    return projectionStrategy.injectContent(injector);
  }
}
