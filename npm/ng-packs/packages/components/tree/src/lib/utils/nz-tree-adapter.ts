export abstract class BaseNode {
  name?: string;
  displayName?: string;

  constructor(public id: string, public parentId: string | null) {}
}

class TreeNode<T extends BaseNode> extends BaseNode {
  title: string | undefined;
  key: string;
  icon: string | null = null;
  children: TreeNode<T>[] = [];
  isLeaf = true;
  checked = false;
  selected = false;
  expanded = false;
  selectable = true;
  disabled = false;
  disableCheckbox = false;
  parentNode?: TreeNode<T> | null;

  constructor(public entity: T, private nameResolver = ent => ent.displayName || ent.name) {
    super(entity.id, entity.parentId);
    this.key = entity.id;
    this.title = nameResolver(entity);
  }
}

export class TreeAdapter<T extends BaseNode = BaseNode> {
  private tree: TreeNode<T>[];

  constructor(private list: T[] = []) {
    this.tree = createTreeFromList(this.list);
  }

  getList() {
    return this.list;
  }

  getTree() {
    return this.tree;
  }

  handleDrop({ key, parentNode }: TreeNode<T>) {
    const index = this.list.findIndex(({ id }) => id === key);
    this.list[index].parentId = parentNode ? parentNode.key : null;
    this.tree = createTreeFromList(this.list);
  }

  handleRemove({ key }: TreeNode<T>) {
    this.updateTreeFromList(this.list.filter(item => item.id !== key));
  }

  handleUpdate({ key, children }: { key: string; children: T[] }) {
    /**
     * When we need to update a node with new children, first we need to remove any descendant nodes.
     * If we remove immediate children and create a new tree, any other descendant nodes will be removed
     * and we won't need to recursively remove sub children.
     * Then, you simply add back the new children and create a new tree.
     */
    const listWithDescendantNodesRemoved = this.updateTreeFromList(
      this.list.filter(item => item.parentId !== key),
    );
    this.updateTreeFromList(listWithDescendantNodesRemoved.concat(children));
  }

  updateTreeFromList(list: T[]) {
    this.tree = createTreeFromList(list);
    this.list = createListFromTree(this.tree);
    return this.list;
  }
}

// UTILITY FUNCTIONS

function createTreeFromList<T extends BaseNode>(list: T[]): TreeNode<T>[] {
  const map = createMapFromList(list);
  const tree: TreeNode<T>[] = [];

  list.forEach(row => {
    const parentId = row.parentId;
    const node = map.get(row.id);
    if (parentId) {
      const parent = map.get(parentId);
      if (!parent) return;
      parent.children.push(node);
      parent.isLeaf = false;
    } else {
      tree.push(node);
    }
  });

  return tree;
}

function createListFromTree<T extends BaseNode>(tree: TreeNode<T>[], list: T[] = []): T[] {
  tree.forEach(node => {
    list.push({ ...node.entity, parentId: node.parentId });
    if (node.children) createListFromTree(node.children, list);
  });

  return list;
}

function createMapFromList<T extends BaseNode>(
  list: T[],
  map = new Map<string, TreeNode<T>>(),
): Map<string, TreeNode<T>> {
  list.forEach(row => map.set(row.id, new TreeNode(row)));

  return map;
}
