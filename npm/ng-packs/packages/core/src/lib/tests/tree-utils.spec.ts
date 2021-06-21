import {
  BaseTreeNode,
  createTreeFromList,
  createTreeNodeFilterCreator,
  TreeNode,
} from '../utils/tree-utils';

const LIST_1 = [
  { id: 1, pid: null },
  { id: 2, pid: 1 },
];
const LIST_2 = [
  { id: 1, pid: null },
  { id: 2, pid: 1 },
  { id: 3, pid: 1 },
];
const LIST_3 = [
  { id: 1, pid: null },
  { id: 2, pid: 1 },
  { id: 3, pid: 2 },
];
const TREE_1 = [
  { id: 1, pid: null, isLeaf: false, children: [{ id: 2, pid: 1, isLeaf: true, children: [] }] },
];
const TREE_2 = [
  {
    id: 1,
    pid: null,
    isLeaf: false,
    children: [
      { id: 2, pid: 1, isLeaf: true, children: [] },
      { id: 3, pid: 1, isLeaf: true, children: [] },
    ],
  },
];
const TREE_3 = [
  {
    id: 1,
    pid: null,
    isLeaf: false,
    children: [
      { id: 2, pid: 1, isLeaf: false, children: [{ id: 3, pid: 2, isLeaf: true, children: [] }] },
    ],
  },
];
const SOURCE_TREE: TreeNode<SearchModel>[] = [
  {
    id: 1,
    pid: null,
    isLeaf: false,
    name: 'foo',
    children: [
      {
        id: 2,
        pid: 1,
        name: 'bar',
        isLeaf: false,
        children: [{ id: 3, pid: 2, name: 'qux', isLeaf: true, children: [] }],
      },
      { id: 4, pid: 1, name: 'baz', isLeaf: true, children: [] },
      { id: 5, pid: 1, name: 'quux', isLeaf: true, children: [] },
    ],
  },
];
const RESULT_TREE_1 = [
  { id: 3, pid: 2, name: 'qux', isLeaf: true, children: [] },
  { id: 5, pid: 1, name: 'quux', isLeaf: true, children: [] },
];
const RESULT_TREE_2 = [{ id: 5, pid: 1, name: 'quux', isLeaf: true, children: [] }];
const RESULT_TREE_3 = [
  {
    id: 2,
    pid: 1,
    name: 'bar',
    isLeaf: false,
    children: [{ id: 3, pid: 2, name: 'qux', isLeaf: true, children: [] }],
  },
  { id: 4, pid: 1, name: 'baz', isLeaf: true, children: [] },
];
const RESULT_TREE_4 = [{ id: 4, pid: 1, name: 'baz', isLeaf: true, children: [] }];

describe('Tree Utils', () => {
  describe('createTreeFromList', () => {
    test.each`
      list      | expected
      ${LIST_1} | ${TREE_1}
      ${LIST_2} | ${TREE_2}
      ${LIST_3} | ${TREE_3}
    `('should return $expected when given $list', ({ list, expected }: TestCreateTreeFromList) => {
      const tree = createTreeFromList(
        list,
        x => x.id,
        x => x.pid,
        x => BaseTreeNode.create(x),
      );

      expect(removeParents(tree)).toEqual(expected);
    });
  });

  describe('createTreeNodeFilterCreator', () => {
    test.each`
      search   | expected
      ${'qu'}  | ${RESULT_TREE_1}
      ${'quu'} | ${RESULT_TREE_2}
      ${'ba'}  | ${RESULT_TREE_3}
      ${'baz'} | ${RESULT_TREE_4}
    `(
      'should return $expected when $search is searched',
      ({ search, expected }: TestCreateTreeNodeFilter) => {
        const filter = createTreeNodeFilterCreator('name', String)(search);

        expect(filter(SOURCE_TREE)).toEqual(expected);
      },
    );
  });
});

function removeParents(tree: TreeNode<Model>[]) {
  return tree.map(v => {
    const { parent, ...node } = v;
    node.children = removeParents(node.children);
    return node;
  });
}

interface TestCreateTreeFromList {
  list: Model[];
  expected: TreeNode<Model>[];
}

interface Model {
  id: number;
  pid?: number;
}

interface TestCreateTreeNodeFilter {
  search: string;
  expected: TreeNode<SearchModel>[];
}

interface SearchModel {
  id: number;
  pid?: number;
  name: string;
}
