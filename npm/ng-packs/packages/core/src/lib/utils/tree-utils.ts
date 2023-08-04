import { isArray } from './common-utils';

/* eslint-disable @typescript-eslint/ban-types */
export class BaseTreeNode<T extends object> {
  children: TreeNode<T>[] = [];
  isLeaf = true;
  parent?: TreeNode<T>;

  constructor(props: T) {
    Object.assign(this, props);
  }

  static create<T extends object>(props: T) {
    return new BaseTreeNode<T>(props) as TreeNode<T>;
  }
}

export function createTreeFromList<T extends object, R>(
  list: T[],
  keySelector: (item: T) => NodeKey,
  parentKeySelector: typeof keySelector,
  valueMapper: (item: T) => R,
) {
  const map = createMapFromList(list, keySelector, valueMapper);
  const tree: NodeValue<T, typeof valueMapper>[] = [];

  list.forEach(row => {
    const id = keySelector(row);
    const parentId = parentKeySelector(row);
    const node = map.get(id);

    if (!node) return;

    if (parentId) {
      const parent = map.get(parentId);
      if (!parent) return;
      (parent as any).children.push(node);
      (parent as any).isLeaf = false;
      (node as any).parent = parent;
    } else {
      tree.push(node);
    }
  });

  return tree;
}

export function createMapFromList<T extends object, R>(
  list: T[],
  keySelector: (item: T) => NodeKey,
  valueMapper: (item: T) => R,
) {
  type Key = ReturnType<typeof keySelector>;
  type Value = NodeValue<T, typeof valueMapper>;
  const map = new Map<Key, Value>();
  list.forEach(row => map.set(keySelector(row), valueMapper(row)));
  return map;
}

export function createTreeNodeFilterCreator<T extends object>(
  key: keyof T,
  mapperFn: (value: any) => string,
) {
  return (search: string) => {
    const regex = new RegExp('.*' + search + '.*', 'i');

    return function collectNodes(nodes: TreeNode<T>[], matches: TreeNode<T>[] = []) {
      for (const node of nodes) {
        if (regex.test(mapperFn(node[key]))) matches.push(node);

        if (node.children.length) collectNodes(node.children, matches);
      }

      return matches;
    };
  };
}

export function createGroupMap<T extends { group?: string }>(
  list: TreeNode<T>[],
  othersGroupKey: string,
) {
  if (!isArray(list) || !list.some(node => Boolean(node.group))) return undefined;

  const mapGroup = new Map<string, TreeNode<T>[]>();

  for (const node of list) {
    const group = node?.group || othersGroupKey;
    if (typeof group !== 'string') {
      throw new Error(`Invalid group: ${group}`);
    }

    const items = mapGroup.get(group) || [];
    items.push(node);
    mapGroup.set(group, items);
  }

  return mapGroup;
}

export type TreeNode<T extends object> = {
  [K in keyof T]: T[K];
} & {
  children: TreeNode<T>[];
  isLeaf: boolean;
  parent?: TreeNode<T>;
};

export type RouteGroup<T extends object> = {
  readonly group: string;
  readonly items: TreeNode<T>[];
};

export type NodeKey = number | string | symbol | undefined | null;

export type NodeValue<T extends object, F extends (...args: any) => any> = F extends undefined
  ? TreeNode<T>
  : ReturnType<F>;
