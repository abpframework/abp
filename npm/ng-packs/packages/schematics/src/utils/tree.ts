export class TypeNode {
  children: TypeNode[] = [];

  index = 0;

  constructor(
    public data: string,
    public parent: TypeNode | null,
    public mapperFn = (node: TypeNode) => node.data,
  ) {}

  toGenerics(): string[] {
    const generics = this.children.length ? `<${this.children.map(n => `T${n.index}`)}>` : '';
    return [this.data + generics].concat(
      this.children.reduce((acc: string[], node) => acc.concat(node.toGenerics()), []),
    );
  }

  toString() {
    const self = this.mapperFn(this);

    if (!self) return '';

    const representation = self + this.children.filter(String).join(', ');

    if (!this.parent) return representation;

    const siblings = this.parent.children;

    return (
      (siblings[0] === this ? '<' : '') +
      representation +
      (siblings[siblings.length - 1] === this ? '>' : '')
    );
  }

  valueOf() {
    return this.toString();
  }
}

export function parseGenerics(type: string, mapperFn?: TypeNodeMapperFn) {
  const [rootType, ...types] = type.split('<');
  const root = new TypeNode(rootType, null, mapperFn);

  types.reduce((parent, t) => {
    const [left, right] = t.split(/>+,?\s*/);

    const leftNode = new TypeNode(left, parent, mapperFn);
    leftNode.index = parent.children.length;
    parent.children.push(leftNode);
    parent = leftNode;

    let { length } = t.match(/>/g) || [];
    while (length--) parent = parent.parent!;

    if (right) {
      parent = parent.parent!;
      const rightNode = new TypeNode(right, parent, mapperFn);
      rightNode.index = parent.children.length;
      parent.children.push(rightNode);
      parent = rightNode;
    }

    return parent;
  }, root);

  return root;
}

export type TypeNodeMapperFn = (node: TypeNode) => string;
