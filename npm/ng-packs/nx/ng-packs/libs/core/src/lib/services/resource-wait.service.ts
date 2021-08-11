import { Injectable } from '@angular/core';
import { InternalStore } from '../utils/internal-store-utils';

export interface ResourceWaitState {
  resources: Set<string>;
}

@Injectable({
  providedIn: 'root',
})
export class ResourceWaitService {
  private store = new InternalStore<ResourceWaitState>({ resources: new Set() });

  getLoading() {
    return !!this.store.state.resources.size;
  }

  getLoading$() {
    return this.store.sliceState(({ resources }) => !!resources.size);
  }

  updateLoading$() {
    return this.store.sliceUpdate(({ resources }) => !!resources.size);
  }

  clearLoading() {
    this.store.patch({ resources: new Set() });
  }

  addResource(resource: string) {
    const resources = this.store.state.resources;
    resources.add(resource);
    this.store.patch({ resources });
  }

  deleteResource(resource: string) {
    const resources = this.store.state.resources;
    resources.delete(resource);
    this.store.patch({ resources });
  }
}
