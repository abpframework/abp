import { Injectable, OnDestroy } from '@angular/core';
import { Actions, ofActionSuccessful, Store } from '@ngxs/store';
import { BehaviorSubject, Observable } from 'rxjs';
import { GetAppConfiguration } from '../actions/config.actions';
import { ABP } from '../models/common';
import { ConfigState } from '../states/config.state';
import { takeUntilDestroy } from '../utils/rxjs-utils';
import { pushValueTo } from '../utils/array-utils';
import { BaseTreeNode, createTreeFromList, TreeNode } from '../utils/tree-utils';

export abstract class AbstractTreeService<T extends object> {
  abstract id: string;
  abstract parentId: string;
  abstract hide: (item: T) => boolean;
  abstract sort: (a: T, b: T) => number;

  private _flat$ = new BehaviorSubject<T[]>([]);
  private _tree$ = new BehaviorSubject<TreeNode<T>[]>([]);
  private _visible$ = new BehaviorSubject<TreeNode<T>[]>([]);

  get flat(): T[] {
    return this._flat$.value;
  }

  get flat$(): Observable<T[]> {
    return this._flat$.asObservable();
  }

  get tree(): TreeNode<T>[] {
    return this._tree$.value;
  }

  get tree$(): Observable<TreeNode<T>[]> {
    return this._tree$.asObservable();
  }

  get visible(): TreeNode<T>[] {
    return this._visible$.value;
  }

  get visible$(): Observable<TreeNode<T>[]> {
    return this._visible$.asObservable();
  }

  protected createTree(items: T[]): TreeNode<T>[] {
    return createTreeFromList<T, TreeNode<T>>(
      items,
      item => item[this.id],
      item => item[this.parentId],
      item => BaseTreeNode.create(item),
    );
  }

  private filterWith(setOrMap: Set<string> | Map<string, T>): T[] {
    return this._flat$.value.filter(
      item => !setOrMap.has(item[this.id]) && !setOrMap.has(item[this.parentId]),
    );
  }

  private publish(flatItems: T[], visibleItems: T[]): T[] {
    this._flat$.next(flatItems);
    this._tree$.next(this.createTree(flatItems));
    this._visible$.next(this.createTree(visibleItems));
    return flatItems;
  }

  add(items: T[]): T[] {
    const map = new Map<string, T>();
    items.forEach(item => map.set(item[this.id], item));

    const flatItems = this.filterWith(map);
    map.forEach(pushValueTo(flatItems));

    flatItems.sort(this.sort);
    const visibleItems = flatItems.filter(item => !this.hide(item));

    return this.publish(flatItems, visibleItems);
  }

  find(predicate: (item: TreeNode<T>) => boolean, tree = this.tree): TreeNode<T> | null {
    return tree.reduce(
      (acc, node) => (acc ? acc : predicate(node) ? node : this.find(predicate, node.children)),
      null,
    );
  }

  patch(identifier: string, props: Partial<T>): T[] | false {
    const flatItems = this._flat$.value;
    const index = flatItems.findIndex(item => item[this.id] === identifier);
    if (index < 0) return false;

    flatItems[index] = { ...flatItems[index], ...props };

    flatItems.sort(this.sort);
    const visibleItems = flatItems.filter(item => !this.hide(item));

    return this.publish(flatItems, visibleItems);
  }

  refresh(): T[] {
    return this.add([]);
  }

  remove(identifiers: string[]): T[] {
    const set = new Set<string>();
    identifiers.forEach(id => set.add(id));

    const flatItems = this.filterWith(set);
    const visibleItems = flatItems.filter(item => !this.hide(item));

    return this.publish(flatItems, visibleItems);
  }

  search(params: Partial<T>, tree = this.tree): TreeNode<T> | null {
    const searchKeys = Object.keys(params);

    return tree.reduce(
      (acc, node) =>
        acc
          ? acc
          : searchKeys.every(key => node[key] === params[key])
          ? node
          : this.search(params, node.children),
      null,
    );
  }
}

@Injectable()
export abstract class AbstractNavTreeService<T extends ABP.Nav> extends AbstractTreeService<T>
  implements OnDestroy {
  readonly id = 'name';
  readonly parentId = 'parentName';
  readonly hide = (item: T) => item.invisible || !this.isGranted(item);
  readonly sort = (a: T, b: T) => {
    if (!a.order) return 1;
    if (!b.order) return -1;

    return a.order - b.order;
  };

  constructor(protected actions: Actions, protected store: Store) {
    super();

    this.actions
      .pipe(takeUntilDestroy(this), ofActionSuccessful(GetAppConfiguration))
      .subscribe(() => this.refresh());
  }

  protected isGranted({ requiredPolicy }: T): boolean {
    return this.store.selectSnapshot(ConfigState.getGrantedPolicy(requiredPolicy));
  }

  hasChildren(identifier: string): boolean {
    const node = this.find(item => item[this.id] === identifier);
    return Boolean(node?.children?.length);
  }

  hasInvisibleChild(identifier: string): boolean {
    const node = this.find(item => item[this.id] === identifier);
    return node?.children?.some(child => child.invisible);
  }

  /* istanbul ignore next */
  ngOnDestroy() {}
}

@Injectable({ providedIn: 'root' })
export class RoutesService extends AbstractNavTreeService<ABP.Route> {}

@Injectable({ providedIn: 'root' })
export class SettingTabsService extends AbstractNavTreeService<ABP.Tab> {}
