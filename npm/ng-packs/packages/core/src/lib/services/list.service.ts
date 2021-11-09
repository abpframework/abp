import { Injectable, Injector, OnDestroy } from '@angular/core';
import {
  BehaviorSubject,
  MonoTypeOperatorFunction,
  Observable,
  of,
  ReplaySubject,
  Subject,
} from 'rxjs';
import {
  catchError,
  debounceTime,
  filter,
  shareReplay,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs/operators';
import { ABP } from '../models/common';
import { PagedResultDto } from '../models/dtos';
import { LIST_QUERY_DEBOUNCE_TIME } from '../tokens/list.token';

@Injectable()
export class ListService<QueryParamsType = ABP.PageQueryParams | any> implements OnDestroy {
  private _filter = '';
  set filter(value: string) {
    this._filter = value;
    this.get();
  }
  get filter(): string {
    return this._filter;
  }

  private _maxResultCount = 10;
  set maxResultCount(value: number) {
    this._maxResultCount = value;
    this.get();
  }
  get maxResultCount(): number {
    return this._maxResultCount;
  }

  private _skipCount = 0;
  private _page = 0;
  set page(value: number) {
    if (value === this._page) return;

    this._page = value;
    this.get();
  }
  get page(): number {
    return this._page;
  }

  private _sortKey = '';
  set sortKey(value: string) {
    this._sortKey = value;
    this.get();
  }
  get sortKey(): string {
    return this._sortKey;
  }

  private _sortOrder = '';
  set sortOrder(value: string) {
    this._sortOrder = value;
    this.get();
  }
  get sortOrder(): string {
    return this._sortOrder;
  }

  private _query$ = new ReplaySubject<QueryParamsType>(1);

  get query$(): Observable<QueryParamsType> {
    return this._query$
      .asObservable()
      .pipe(this.delay, shareReplay({ bufferSize: 1, refCount: true }));
  }

  private _isLoading$ = new BehaviorSubject(false);

  private destroy$ = new Subject();

  private delay: MonoTypeOperatorFunction<QueryParamsType>;

  get isLoading$(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  get = () => {
    this.resetPageWhenUnchanged();
    this.next();
  };

  getWithoutPageReset = () => {
    this.next();
  };

  constructor(injector: Injector) {
    const delay = injector.get(LIST_QUERY_DEBOUNCE_TIME, 300);
    this.delay = delay ? debounceTime(delay) : tap();
    this.get();
  }

  hookToQuery<T extends any>(
    streamCreatorCallback: QueryStreamCreatorCallback<T, QueryParamsType>,
  ): Observable<PagedResultDto<T>> {
    this._isLoading$.next(true);

    return this.query$.pipe(
      switchMap(query => streamCreatorCallback(query).pipe(catchError(() => of(null)))),
      filter(Boolean),
      tap(() => this._isLoading$.next(false)),
      shareReplay<any>({ bufferSize: 1, refCount: true }),
      takeUntil(this.destroy$),
    );
  }

  ngOnDestroy() {
    this.destroy$.next();
  }

  private resetPageWhenUnchanged() {
    const skipCount = this._page * this._maxResultCount;

    if (skipCount === this._skipCount) {
      this._page = 0;
      this._skipCount = 0;
    } else this._skipCount = skipCount;
  }

  private next() {
    this._query$.next({
      filter: this._filter || undefined,
      maxResultCount: this._maxResultCount,
      skipCount: this._page * this._maxResultCount,
      sorting: this._sortOrder ? `${this._sortKey} ${this._sortOrder}` : undefined,
    } as any as QueryParamsType);
  }
}

export type QueryStreamCreatorCallback<T, QueryParamsType = ABP.PageQueryParams> = (
  query: QueryParamsType,
) => Observable<PagedResultDto<T>>;
