import { Injectable, Injector } from '@angular/core';
import { HttpRequest } from '@angular/common/http';
import { InternalStore } from '../utils/internal-store-utils';
import { getPathName } from '../utils/http-utils';
import { map, mapTo, switchMap, takeUntil, tap } from 'rxjs/operators';
import { of, Subject, timer } from 'rxjs';
import { LOADER_DELAY } from '../tokens/lodaer-delay.token';

export interface HttpWaitState {
  requests: HttpRequest<any>[];
  filteredRequests: Array<HttpRequestInfo>;
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
    requests: [],
    filteredRequests: [],
  });

  private delay: number;
  private destroy$ = new Subject();

  constructor(injector: Injector) {
    this.delay = injector.get(LOADER_DELAY, 500);
  }

  getLoading() {
    return !!this.applyFilter(this.store.state.requests).length;
  }

  getLoading$() {
    return this.store
      .sliceState(({ requests }) => requests)
      .pipe(
        map(requests => !!this.applyFilter(requests).length),
        switchMap(condition =>
          condition
            ? this.delay === 0
              ? of(true)
              : timer(this.delay).pipe(mapTo(true), takeUntil(this.destroy$))
            : of(false),
        ),
        tap(() => this.destroy$.next()),
      );
  }

  updateLoading$() {
    return this.store.sliceUpdate(({ requests }) => !!this.applyFilter(requests).length);
  }

  clearLoading() {
    this.store.patch({ requests: [] });
  }

  addRequest(request: HttpRequest<any>) {
    this.store.patch({ requests: [...this.store.state.requests, request] });
  }

  deleteRequest(request: HttpRequest<any>) {
    const requests = this.store.state.requests.filter(r => r !== request);
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

  private applyFilter(requests: HttpRequest<any>[]) {
    const { filteredRequests } = this.store.state;
    return requests.filter(
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
}
