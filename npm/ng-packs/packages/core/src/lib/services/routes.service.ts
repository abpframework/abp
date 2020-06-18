import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { BehaviorSubject, Observable } from 'rxjs';
import { ABP } from '../models/common';
import { ConfigState } from '../states/config.state';
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

  patch(identifier: string, props: Partial<T>): T[] | false {
    const flatItems = this._flat$.value;
    const index = flatItems.findIndex(item => item[this.id] === identifier);
    if (index < 0) return false;

    flatItems[index] = { ...flatItems[index], ...props };

    flatItems.sort(this.sort);
    const visibleItems = flatItems.filter(item => !this.hide(item));

    return this.publish(flatItems, visibleItems);
  }

  remove(identifiers: string[]): T[] {
    const set = new Set<string>();
    identifiers.forEach(id => set.add(id));

    const flatItems = this.filterWith(set);
    const visibleItems = flatItems.filter(item => !this.hide(item));

    return this.publish(flatItems, visibleItems);
  }

  search(params: Partial<T>, tree = this.tree): TreeNode<T> {
    const searchKeys = Object.keys(params);

    return tree.reduce(
      (acc, node) =>
        acc
          ? acc
          : searchKeys.every(key => node[key] === params[key])
          ? node
          : node.children
          ? this.search(params, node.children)
          : acc,
      null,
    );
  }
}

@Injectable({
  providedIn: 'root',
})
export class RoutesService extends AbstractTreeService<ABP.Route> {
  readonly id = 'name';
  readonly parentId = 'parentName';
  readonly hide = (item: ABP.Route) => item.invisible;
  readonly sort = (a: ABP.Route, b: ABP.Route) => a.order - b.order;
}

@Injectable({
  providedIn: 'root',
})
export class SettingTabsService extends AbstractTreeService<ABP.Tab> {
  readonly id = 'name';
  readonly parentId = 'parentName';
  readonly hide = (setting: ABP.Tab) => setting.invisible || !this.isGranted(setting);
  readonly sort = (a: ABP.Tab, b: ABP.Tab) => a.order - b.order;

  constructor(private store: Store) {
    super();
  }

  private isGranted(setting: ABP.Tab): boolean {
    return this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy));
  }
}
