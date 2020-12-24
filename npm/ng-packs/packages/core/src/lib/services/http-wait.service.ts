import { Injectable } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { InternalStore } from '../utils/internal-store-utils';

export interface HttpWaitState {
  requests: Set<HttpRequest<any>>;
}

@Injectable({
  providedIn: 'root',
})
export class HttpWaitService {
  protected store = new InternalStore<HttpWaitState>({ requests: new Set() });

  getLoading() {
    return !!this.store.state.requests.size;
  }

  getLoading$() {
    return this.store.sliceState(({ requests }) => !!requests.size);
  }

  updateLoading$() {
    return this.store.sliceUpdate(({ requests }) => !!requests.size);
  }

  clearLoading() {
    this.store.patch({ requests: new Set() });
  }

  addRequest(request: HttpRequest<any>) {
    const requests = this.store.state.requests;
    requests.add(request);
    this.store.patch({ requests });
  }

  deleteRequest(request: HttpRequest<any>) {
    const requests = this.store.state.requests;
    requests.delete(request);
    this.store.patch({ requests });
  }

  // TODO: Add filter function
}
