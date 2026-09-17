import { BehaviorSubject, Observable } from 'rxjs';
import { NavItem } from '../models/nav-item';
import { inject, Type } from '@angular/core';
import { SORT_COMPARE_FUNC, SortableItem } from '@abp/ng.core';

export abstract class AbstractMenuService<T extends NavItem> {
  protected abstract baseClass: Type<any>;

  protected readonly sortFn: (a: SortableItem, b: SortableItem) => number;

  protected _items$ = new BehaviorSubject<T[]>([]);

  get items(): T[] {
    return this._items$.value;
  }

  get items$(): Observable<T[]> {
    return this._items$.asObservable();
  }

  constructor() {
    this.sortFn = inject(SORT_COMPARE_FUNC);
  }

  addItems(newItems: T[]) {
    const items = [...this.items];
    newItems.forEach(item => {
      const index = items.findIndex(i => i.id === item.id);
      const data = new this.baseClass(item);

      if (index > -1) {
        items[index] = data;
        return;
      }

      items.push(data);
    });
    items.sort(this.sortItems);
    this._items$.next(items);
  }

  removeItem(id: string | number) {
    const index = this.items.findIndex(item => item.id === id);

    if (index < 0) return;

    const items = [...this.items.slice(0, index), ...this.items.slice(index + 1)];
    this._items$.next(items);
  }

  patchItem(id: string | number, item: Partial<Omit<T, 'id'>>) {
    const index = this.items.findIndex(i => i.id === id);

    if (index < 0) return;

    const items = [...this.items];
    items[index] = new this.baseClass({ ...items[index], ...item });
    items.sort(this.sortItems);
    this._items$.next(items);
  }

  sortItems = (a: T, b: T) => {
    return this.sortFn(a, b);
  };
}
