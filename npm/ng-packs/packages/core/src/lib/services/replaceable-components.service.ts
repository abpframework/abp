import { Injectable, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ReplaceableComponents } from '../models/replaceable-components';
import { InternalStore } from '../utils/internal-store-utils';
import { reloadRoute } from '../utils/route-utils';

@Injectable({ providedIn: 'root' })
export class ReplaceableComponentsService {
  private readonly store: InternalStore<ReplaceableComponents.ReplaceableComponent[]>;

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

    this.store.set(replaceableComponents);

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
