import { BehaviorSubject, Observable } from 'rxjs';
import { NavItem } from '../models/nav-item';

export abstract class AbstractMenuService<T extends NavItem> {
  protected abstract baseClass;

  protected _items$ = new BehaviorSubject<T[]>([]);

  get items(): T[] {
    return this._items$.value;
  }

  get items$(): Observable<T[]> {
    return this._items$.asObservable();
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

  private sortItems(a: T, b: T) {
    if (!a.order) return 1;
    if (!b.order) return -1;

    return a.order - b.order;
  }
}
