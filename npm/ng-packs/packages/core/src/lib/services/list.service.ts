import { Inject, Injectable, OnDestroy, Optional } from '@angular/core';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { debounceTime, shareReplay, switchMap, tap } from 'rxjs/operators';
import { ABP } from '../models/common';
import { PagedResultDto } from '../models/dtos';
import { LIST_QUERY_DEBOUNCE_TIME } from '../tokens/list.token';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Injectable()
export class ListService implements OnDestroy {
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

  private _page = 1;
  set page(value: number) {
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

  private _query$ = new ReplaySubject<ABP.PageQueryParams>(1);

  get query$(): Observable<ABP.PageQueryParams> {
    return this._query$
      .asObservable()
      .pipe(debounceTime(this.delay || 300), shareReplay({ bufferSize: 1, refCount: true }));
  }

  private _isLoading$ = new BehaviorSubject(false);

  get isLoading$(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  get = () => {
    this._query$.next({
      filter: this._filter || undefined,
      maxResultCount: this._maxResultCount,
      skipCount: (this._page - 1) * this._maxResultCount,
      sorting: this._sortOrder ? `${this._sortKey} ${this._sortOrder}` : undefined,
    });
  };

  constructor(@Optional() @Inject(LIST_QUERY_DEBOUNCE_TIME) private delay: number) {
    this.get();
  }

  hookToQuery<T extends any>(
    streamCreatorCallback: QueryStreamCreatorCallback<T>,
  ): Observable<PagedResultDto<T>> {
    this._isLoading$.next(true);

    return this.query$.pipe(
      switchMap(streamCreatorCallback),
      tap(() => this._isLoading$.next(false)),
      shareReplay({ bufferSize: 1, refCount: true }),
      takeUntilDestroy(this),
    );
  }

  ngOnDestroy() {}
}

export type QueryStreamCreatorCallback<T> = (
  query: ABP.PageQueryParams,
) => Observable<PagedResultDto<T>>;
