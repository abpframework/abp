import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { NavItem } from '../models/nav-item';

@Injectable({ providedIn: 'root' })
export class NavItemsService {
  private _items$ = new BehaviorSubject<NavItem[]>([]);

  get items(): NavItem[] {
    return this._items$.value;
  }

  get items$(): Observable<NavItem[]> {
    return this._items$.asObservable();
  }

  addItems(items: NavItem[]) {
    this._items$.next([...this.items, ...items].sort(sortItems));
  }

  removeItem(id: string | number) {
    const index = this.items.findIndex(item => item.id === id);

    if (index > -1) {
      this._items$.next([...this.items.slice(0, index), ...this.items.slice(index + 1)]);
    }
  }

  patchItem(id: string | number, item: Partial<Omit<NavItem, 'id'>>) {
    const index = this.items.findIndex(i => i.id === id);

    if (index > -1) {
      this._items$.next(
        [
          ...this.items.slice(0, index),
          { ...this.items[index], ...item },
          ...this.items.slice(index + 1),
        ].sort(sortItems),
      );
    }
  }
}

function sortItems(a: NavItem, b: NavItem) {
  if (!a.order) return 1;
  if (!b.order) return -1;

  return a.order - b.order;
}
