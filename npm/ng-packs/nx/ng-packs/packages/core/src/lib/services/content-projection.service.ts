import { Injectable, Injector, TemplateRef, Type } from '@angular/core';
import { ProjectionStrategy } from '../strategies/projection.strategy';

@Injectable({ providedIn: 'root' })
export class ContentProjectionService {
  constructor(private injector: Injector) {}

  projectContent<T extends Type<any> | TemplateRef<any>>(
    projectionStrategy: ProjectionStrategy<T>,
    injector = this.injector,
  ) {
    return projectionStrategy.injectContent(injector);
  }
}
