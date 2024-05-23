import { Injectable, Injector, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subscription, map } from 'rxjs';
import { ABP } from '../models/common';
import { OTHERS_GROUP } from '../tokens';
import { pushValueTo } from '../utils/array-utils';
import {
  BaseTreeNode,
  createTreeFromList,
  TreeNode,
  RouteGroup,
  createGroupMap,
} from '../utils/tree-utils';
import { ConfigStateService } from './config-state.service';
import { PermissionService } from './permission.service';
import { SORT_COMPARE_FUNC } from '../tokens/compare-func.token';

// eslint-disable-next-line @typescript-eslint/ban-types
export abstract class AbstractTreeService<T extends { [key: string | number | symbol]: any }> {
  abstract id: string;
  abstract parentId: string;
  abstract hide: (item: T) => boolean;
  abstract sort: (a: T, b: T) => number;

  private _flat$ = new BehaviorSubject<T[]>([]);
  private _tree$ = new BehaviorSubject<TreeNode<T>[]>([]);
  private _visible$ = new BehaviorSubject<TreeNode<T>[]>([]);

  protected othersGroup: string;
  protected shouldSingularizeRoutes = true;

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

  private filterWith(setOrMap: Set<string> | Map<string, T>): T[] {
    return this._flat$.value.filter(item => !setOrMap.has(item[this.id]));
  }

  private findItemsToRemove(set: Set<string>): Set<string> {
    return this._flat$.value.reduce((acc, item) => {
      if (!acc.has(item[this.parentId])) {
        return acc;
      }

      const childSet = new Set([item[this.id]]);
      const children = this.findItemsToRemove(childSet);
      return new Set([...acc, ...children]);
    }, set);
  }

  private publish(flatItems: T[]): T[] {
    this._flat$.next(flatItems);
    this._tree$.next(this.createTree(flatItems));
    this._visible$.next(this.createTree(flatItems.filter(item => !this.hide(item))));
    return flatItems;
  }

  protected createTree(items: T[]): TreeNode<T>[] {
    return createTreeFromList<T, TreeNode<T>>(
      items,
      item => item[this.id],
      item => item[this.parentId],
      item => BaseTreeNode.create(item),
    );
  }

  protected createGroupedTree(list: TreeNode<T>[]): RouteGroup<T>[] | undefined {
    const map = createGroupMap<T>(list, this.othersGroup);
    if (!map) {
      return undefined;
    }

    return Array.from(map, ([key, items]) => ({ group: key, items }));
  }

  add(items: T[]): T[] {
    let flatItems: T[] = [];

    if (!this.shouldSingularizeRoutes) {
      flatItems = [...this.flat, ...items];
    }

    if (this.shouldSingularizeRoutes) {
      const map = new Map<string, T>();
      items.forEach(item => map.set(item[this.id], item));
      flatItems = this.filterWith(map);
      map.forEach(pushValueTo(flatItems));
    }

    flatItems.sort(this.sort);

    return this.publish(flatItems);
  }

  find(predicate: (item: TreeNode<T>) => boolean, tree = this.tree): TreeNode<T> | null {
    return tree.reduce<TreeNode<T> | null>((acc, node) => {
      if (acc) {
        return acc;
      }

      if (predicate(node)) {
        return node;
      }

      return this.find(predicate, node.children);
    }, null);
  }

  patch(identifier: string, props: Partial<T>): T[] | false {
    const flatItems = this._flat$.value;
    const index = flatItems.findIndex(item => item[this.id] === identifier);
    if (index < 0) {
      return false;
    }

    flatItems[index] = { ...flatItems[index], ...props };

    flatItems.sort(this.sort);
    return this.publish(flatItems);
  }

  refresh(): T[] {
    return this.add([]);
  }

  remove(identifiers: string[]): T[] {
    const set = new Set<string>();
    identifiers.forEach(id => set.add(id));

    const setToRemove = this.findItemsToRemove(set);
    const flatItems = this.filterWith(setToRemove);
    return this.publish(flatItems);
  }

  removeByParam(params: Partial<T>): T[] | null {
    if (!params) {
      return null;
    }

    const keys = Object.keys(params) as Array<keyof Partial<T>>;
    if (keys.length === 0) {
      return null;
    }

    const excludedList = this.flat.filter(item => keys.every(key => item[key] === params[key]));
    if (!excludedList?.length) {
      return null;
    }

    for (const item of excludedList) {
      this.removeByParam({ [this.parentId]: item[this.id] } as Partial<T>);
    }

    const flatItems = this.flat.filter(item => !excludedList.includes(item));
    return this.publish(flatItems);
  }

  search(params: Partial<T>, tree = this.tree): TreeNode<T> | null {
    const searchKeys = Object.keys(params) as Array<keyof Partial<T>>;

    return tree.reduce<TreeNode<T> | null>((acc, node) => {
      if (acc) {
        return acc;
      }

      if (searchKeys.every(key => node[key] === params[key])) {
        return node;
      }

      return this.search(params, node.children);
    }, null);
  }

  setSingularizeStatus(singularize = true): void {
    this.shouldSingularizeRoutes = singularize;
  }
}

@Injectable()
export abstract class AbstractNavTreeService<T extends ABP.Nav>
  extends AbstractTreeService<T>
  implements OnDestroy
{
  private subscription: Subscription;
  private permissionService: PermissionService;
  private compareFunc;
  readonly id = 'name';
  readonly parentId = 'parentName';
  readonly hide = (item: T) => item.invisible || !this.isGranted(item);
  readonly sort = (a: T, b: T) => {
    return this.compareFunc(a, b);
  };

  constructor(protected injector: Injector) {
    super();
    const configState = this.injector.get(ConfigStateService);
    this.subscription = configState
      .createOnUpdateStream(state => state)
      .subscribe(() => this.refresh());
    this.permissionService = injector.get(PermissionService);
    this.othersGroup = injector.get(OTHERS_GROUP);
    this.compareFunc = injector.get(SORT_COMPARE_FUNC);
  }

  protected isGranted({ requiredPolicy }: T): boolean {
    return this.permissionService.getGrantedPolicy(requiredPolicy);
  }

  hasChildren(identifier: string): boolean {
    const node = this.find(item => item[this.id] === identifier);
    return Boolean(node?.children?.length);
  }

  hasInvisibleChild(identifier: string): boolean {
    const node = this.find(item => item[this.id] === identifier);
    return node?.children?.some(child => child.invisible) || false;
  }

  /* istanbul ignore next */
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}

@Injectable({ providedIn: 'root' })
export class RoutesService extends AbstractNavTreeService<ABP.Route> {
  private hasPathOrChild(item: TreeNode<ABP.Route>): boolean {
    return Boolean(item.path) || this.hasChildren(item.name);
  }

  get groupedVisible(): RouteGroup<ABP.Route>[] | undefined {
    return this.createGroupedTree(this.visible.filter(item => this.hasPathOrChild(item)));
  }

  get groupedVisible$(): Observable<RouteGroup<ABP.Route>[] | undefined> {
    return this.visible$.pipe(
      map(items => items.filter(item => this.hasPathOrChild(item))),
      map(visible => this.createGroupedTree(visible)),
    );
  }
}
