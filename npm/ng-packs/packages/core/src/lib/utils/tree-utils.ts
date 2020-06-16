export class TreeNode<T extends any> {
  children: TreeNode<T>[] = [];
  isLeaf = true;

  constructor(props: T) {
    Object.assign(this, props);
  }
}

export function createTreeFromList<T extends object, R extends BranchOrLeaf<T>>(
  list: T[],
  keySelector: (item: T) => NodeKey,
  parentKeySelector: (item: T) => NodeKey,
  valueMapper = (item: T) => new TreeNode(item) as R,
) {
  const map = createMapFromList(list, keySelector, valueMapper);
  const tree: ReturnType<typeof valueMapper>[] = [];

  list.forEach(row => {
    const id = keySelector(row);
    const parentId = parentKeySelector(row);
    const node = map.get(id);

    if (parentId) {
      const parent = map.get(parentId);
      parent.children.push(node);
      parent.isLeaf = false;
    } else {
      tree.push(node);
    }
  });

  return tree;
}

export function createMapFromList<T extends object, R extends BranchOrLeaf<T>>(
  list: T[],
  keySelector: (item: T) => NodeKey,
  valueMapper = (item: T) => new TreeNode(item) as R,
) {
  const map = new Map<ReturnType<typeof keySelector>, ReturnType<typeof valueMapper>>();
  list.forEach(row => map.set(keySelector(row), valueMapper(row)));
  return map;
}

type NodeKey = number | string | Symbol;

interface BranchOrLeaf<T> {
  children: BranchOrLeaf<T>[];
  isLeaf: boolean;
}
