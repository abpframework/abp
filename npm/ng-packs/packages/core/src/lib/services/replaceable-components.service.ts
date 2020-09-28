import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaceableComponents } from '../models/replaceable-components';
import { BehaviorSubject, Observable } from 'rxjs';
import { noop } from '../utils/common-utils';
import { map, filter } from 'rxjs/operators';
import { InternalStore } from '../utils/internal-store-utils';
import { reloadRoute } from '../utils/route-utils';

@Injectable({ providedIn: 'root' })
export class ReplaceableComponentsService {
  private store: InternalStore<ReplaceableComponents.ReplaceableComponent[]>;

  get replaceableComponents$(): Observable<ReplaceableComponents.ReplaceableComponent[]> {
    return this.store.sliceState(state => state);
  }

  get replaceableComponents(): ReplaceableComponents.ReplaceableComponent[] {
    return this.store.state;
  }

  get onUpdate$(): Observable<ReplaceableComponents.ReplaceableComponent[]> {
    return this.store.sliceUpdate(state => state);
  }

  constructor(private ngZone: NgZone, private router: Router) {
    this.store = new InternalStore([]);
  }

  add(replaceableComponent: ReplaceableComponents.ReplaceableComponent, reload?: boolean): void {
    const replaceableComponents = [...this.store.state];

    const index = replaceableComponents.findIndex(
      component => component.key === replaceableComponent.key,
    );

    if (index > -1) {
      replaceableComponents[index] = replaceableComponent;
    } else {
      replaceableComponents.push(replaceableComponent);
    }

    this.store.patch(replaceableComponents);

    if (reload) reloadRoute(this.router, this.ngZone);
  }

  get(replaceableComponentKey: string): ReplaceableComponents.ReplaceableComponent {
    return this.replaceableComponents.find(component => component.key === replaceableComponentKey);
  }

  get$(replaceableComponentKey: string): Observable<ReplaceableComponents.ReplaceableComponent> {
    return this.replaceableComponents$.pipe(
      map(components => components.find(component => component.key === replaceableComponentKey)),
    );
  }
}
