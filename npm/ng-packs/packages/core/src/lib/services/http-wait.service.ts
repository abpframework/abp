import { Injectable } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { InternalStore } from '../utils/internal-store-utils';
import { getPathName } from '../utils/http-utils';
import { debounceTime, tap } from 'rxjs/operators';

export interface HttpWaitState {
  requests: Set<HttpRequest<any>>;
  filteredRequests: Array<HttpRequestInfo>;
  delay: number;
}
export interface HttpRequestInfo {
  method: string;
  endpoint: string;
}
@Injectable({
  providedIn: 'root',
})
export class HttpWaitService {
  protected store = new InternalStore<HttpWaitState>({
    requests: new Set(),
    filteredRequests: [],
    delay: 0,
  });

  getLoading() {
    return !!this.applyFilter(this.store.state.requests).length;
  }

  getLoading$() {
    return this.store
      .sliceState(({ requests }) => !!this.applyFilter(requests).length)
      .pipe(this.debounceWhenLoading());
  }

  updateLoading$() {
    return this.store.sliceUpdate(({ requests }) => !!this.applyFilter(requests).length);
  }

  clearLoading() {
    this.store.patch({ requests: new Set() });
  }

  addRequest(request: HttpRequest<any>) {
    let { requests } = this.store.state;
    requests = new Set(requests.values());
    requests.add(request);
    this.store.patch({ requests });
  }

  deleteRequest(request: HttpRequest<any>) {
    const { requests } = this.store.state;
    requests.delete(request);
    this.store.patch({ requests });
  }

  addFilter(request: HttpRequestInfo | HttpRequestInfo[]) {
    const requests = Array.isArray(request) ? request : [request];
    const filteredRequests = [
      ...this.store.state.filteredRequests.filter(
        f => !requests.some(r => this.isSameRequest(f, r)),
      ),
      ...requests,
    ];
    this.store.patch({ filteredRequests });
  }

  removeFilter(request: HttpRequestInfo | HttpRequestInfo[]) {
    const requests = Array.isArray(request) ? request : [request];
    const filteredRequests = this.store.state.filteredRequests.filter(
      f => !requests.some(r => this.isSameRequest(f, r)),
    );
    this.store.patch({ filteredRequests });
  }

  setDelay(delay: number) {
    this.store.patch({ delay });
  }

  private applyFilter(requests: Set<HttpRequest<any>>) {
    const { filteredRequests } = this.store.state;
    return Array.from(requests).filter(
      ({ method, url }) =>
        !filteredRequests.find(filteredRequest =>
          this.isSameRequest(filteredRequest, { method, endpoint: getPathName(url) }),
        ),
    );
  }

  private isSameRequest(filteredRequest: HttpRequestInfo, request: HttpRequestInfo) {
    const { method, endpoint } = filteredRequest;
    return endpoint === request.endpoint && method === request.method;
  }

  private debounceWhenLoading() {
    return this.store.state.delay && !!this.applyFilter(this.store.state.requests).length
      ? debounceTime(this.store.state.delay)
      : tap();
  }
}
