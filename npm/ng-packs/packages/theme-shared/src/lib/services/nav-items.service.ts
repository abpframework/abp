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

  addItems(newItems: NavItem[]) {
    const items = [...this.items];
    newItems.forEach(item => items.push(new NavItem(item)));
    items.sort(sortItems);
    this._items$.next(items);
  }

  removeItem(id: string | number) {
    const index = this.items.findIndex(item => item.id === id);

    if (index < 0) return;

    const items = [...this.items.slice(0, index), ...this.items.slice(index + 1)];
    this._items$.next(items);
  }

  patchItem(id: string | number, item: Partial<Omit<NavItem, 'id'>>) {
    const index = this.items.findIndex(i => i.id === id);

    if (index < 0) return;

    const items = [...this.items];
    items[index] = new NavItem({ ...items[index], ...item });
    items.sort(sortItems);
    this._items$.next(items);
  }
}

function sortItems(a: NavItem, b: NavItem) {
  if (!a.order) return 1;
  if (!b.order) return -1;

  return a.order - b.order;
}
