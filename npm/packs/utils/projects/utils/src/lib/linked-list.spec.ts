import { LinkedList, ListNode } from './linked-list';

describe('Linked List (Doubly)', () => {
  let list: LinkedList;

  beforeEach(() => (list = new LinkedList()));

  describe('#length', () => {
    it('should initially be 0', () => {
      expect(list.length).toBe(0);
    });
  });

  describe('#head', () => {
    it('should initially be undefined', () => {
      expect(list.head).toBeUndefined();
    });
  });

  describe('#tail', () => {
    it('should initially be undefined', () => {
      expect(list.tail).toBeUndefined();
    });
  });

  describe('#add', () => {
    describe('#head', () => {
      it('should add node to the head of the list', () => {
        list.addHead('a');

        // "a"

        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });

      it('should create reference to previous and next nodes', () => {
        list.add('a').head();
        list.add('b').head();
        list.add('c').head();

        // "c" <-> "b" <-> "a"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('c');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('a');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('a');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('c');
        expect(list.tail.next).toBeUndefined();
      });
    });

    describe('#tail', () => {
      it('should add node to the tail of the list', () => {
        list.addTail('a');

        // "a"

        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
        expect(list.tail.next).toBeUndefined();
      });

      it('should create reference to previous and next nodes', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.next).toBeUndefined();
      });
    });

    describe('#after', () => {
      it('should add a node after node with given value', () => {
        list.add('a').tail();

        // "a"

        list.add('b').after('a');
        list.add('c').after('b');

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should add a node to tail if given value is not found', () => {
        list.add('a').tail();

        // "a"

        list.add('b').after('x');

        // "a" <-> "b"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.tail.value).toBe('b');
        expect(list.tail.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.add({ x: 1 }).tail();
        list.add({ x: 2 }).tail();
        list.add({ x: 3 }).tail();

        // {"x":1} <-> {"x":2} <-> {"x":3}

        list.add({ x: 0 }).after({ x: 1 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}

        expect(list.length).toBe(4);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(0);
        expect(list.head.next.next.value.x).toBe(2);
        expect(list.head.next.next.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(2);
        expect(list.tail.previous.previous.value.x).toBe(0);
        expect(list.tail.previous.previous.previous.value.x).toBe(1);
      });
    });

    describe('#before', () => {
      it('should add a node before node with given value', () => {
        list.add('c').tail();

        // "c"

        list.add('b').before('c');
        list.add('a').before('b');

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should add a node to head if given value is not found', () => {
        list.add('b').tail();

        // "a"

        list.add('a').before('x');

        // "a" <-> "b"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.tail.value).toBe('b');
        expect(list.tail.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.add({ x: 1 }).tail();
        list.add({ x: 2 }).tail();
        list.add({ x: 3 }).tail();

        // {"x":1} <-> {"x":2} <-> {"x":3}

        list.add({ x: 0 }).before({ x: 2 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}

        expect(list.length).toBe(4);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(0);
        expect(list.head.next.next.value.x).toBe(2);
        expect(list.head.next.next.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(2);
        expect(list.tail.previous.previous.value.x).toBe(0);
        expect(list.tail.previous.previous.previous.value.x).toBe(1);
      });
    });

    describe('#byIndex', () => {
      it('should add a node at given index', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').byIndex(1);

        // "a" <-> "x" <-> "b" <-> "c"

        list.add('y').byIndex(3);

        // "a" <-> "x" <-> "b" <-> "y" <-> "c"

        expect(list.length).toBe(5);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('x');
        expect(list.head.next.next.value).toBe('b');
        expect(list.head.next.next.next.value).toBe('y');
        expect(list.head.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('y');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.previous.value).toBe('a');
      });

      it('should add a node to head if given index is zero', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').byIndex(0);

        // "x" <-> "a" <-> "b" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('x');
        expect(list.head.next.value).toBe('a');
        expect(list.head.next.next.value).toBe('b');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.previous.previous.previous.value).toBe('x');
      });

      it('should add a node to tail if given index more than size', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').byIndex(4);

        // "a" <-> "b" <-> "c" <-> "x"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.head.next.next.next.value).toBe('x');
        expect(list.tail.value).toBe('x');
        expect(list.tail.previous.value).toBe('c');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('a');
      });

      it('should be able to add a node at given index counting from right to left', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').byIndex(-1);

        // "a" <-> "b" <-> "x" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('x');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('x');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('a');
      });

      it('should add a node to head if given index is less than minus size', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').byIndex(-4);

        // "x" <-> "a" <-> "b" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('x');
        expect(list.head.next.value).toBe('a');
        expect(list.head.next.next.value).toBe('b');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.previous.previous.previous.value).toBe('x');
      });
    });
  });

  describe('#addMany', () => {
    describe('#head', () => {
      it('should add multiple nodes to the head of the list', () => {
        list.add('x').head();

        // "x"

        list.addMany(['a', 'b', 'c']).head();

        // "a" <-> "b" <-> "c" <-> "x"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.head.next.next.next.value).toBe('x');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('x');
        expect(list.tail.previous.value).toBe('c');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('a');
        expect(list.tail.next).toBeUndefined();
      });
    });

    describe('#tail', () => {
      it('should add multiple nodes to the tail of the list', () => {
        list.add('x').tail();

        // "x"

        list.addMany(['a', 'b', 'c']).tail();

        // "x" <-> "a" <-> "b" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('x');
        expect(list.head.next.value).toBe('a');
        expect(list.head.next.next.value).toBe('b');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.previous.previous.previous.value).toBe('x');
        expect(list.tail.next).toBeUndefined();
      });
    });

    describe('#after', () => {
      it('should add multiple nodes after node with given value', () => {
        list.add('a').tail();

        // "a"

        list.addMany(['b', 'c']).after('a');

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y', 'z']).after('b');

        // "a" <-> "b" <-> "x" <-> "y" <-> "z" <-> "c"

        expect(list.length).toBe(6);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('x');
        expect(list.head.next.next.next.value).toBe('y');
        expect(list.head.next.next.next.next.value).toBe('z');
        expect(list.head.next.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('z');
        expect(list.tail.previous.previous.value).toBe('y');
        expect(list.tail.previous.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.previous.previous.value).toBe('a');
      });

      it('should add multiple nodes to tail if given value is not found', () => {
        list.add('a').tail();

        // "a"

        list.addMany(['b', 'c']).after('x');

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

        // {"x":1} <-> {"x":2} <-> {"x":3}

        list.addMany([{ x: 4 }, { x: 5 }]).after({ x: 1 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":4} <-> {"x":5} <-> {"x":2} <-> {"x":3}

        expect(list.length).toBe(5);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(4);
        expect(list.head.next.next.value.x).toBe(5);
        expect(list.head.next.next.next.value.x).toBe(2);
        expect(list.head.next.next.next.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(2);
        expect(list.tail.previous.previous.value.x).toBe(5);
        expect(list.tail.previous.previous.previous.value.x).toBe(4);
        expect(list.tail.previous.previous.previous.previous.value.x).toBe(1);
      });

      it('should not change the list when empty array given as value', () => {
        list.add('a').tail();

        // "a"

        list.addMany([]).after('a');

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });
    });

    describe('#before', () => {
      it('should add multiple nodes before node with given value', () => {
        list.add('c').tail();

        // "c"

        list.addMany(['a', 'b']).before('c');

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y', 'z']).before('b');

        // "a" <-> "x" <-> "y" <-> "z" <-> "b" <-> "c"

        expect(list.length).toBe(6);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('x');
        expect(list.head.next.next.value).toBe('y');
        expect(list.head.next.next.next.value).toBe('z');
        expect(list.head.next.next.next.next.value).toBe('b');
        expect(list.head.next.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('z');
        expect(list.tail.previous.previous.previous.value).toBe('y');
        expect(list.tail.previous.previous.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.previous.previous.value).toBe('a');
      });

      it('should add multiple nodes to head if given value is not found', () => {
        list.add('c').tail();

        // "c"

        list.addMany(['a', 'b']).before('x');

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

        // {"x":1} <-> {"x":2} <-> {"x":3}

        list.addMany([{ x: 4 }, { x: 5 }]).before({ x: 3 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":2} <-> {"x":4} <-> {"x":5} <-> {"x":3}

        expect(list.length).toBe(5);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(2);
        expect(list.head.next.next.value.x).toBe(4);
        expect(list.head.next.next.next.value.x).toBe(5);
        expect(list.head.next.next.next.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(5);
        expect(list.tail.previous.previous.value.x).toBe(4);
        expect(list.tail.previous.previous.previous.value.x).toBe(2);
        expect(list.tail.previous.previous.previous.previous.value.x).toBe(1);
      });

      it('should not change the list when empty array given as value', () => {
        list.add('a').tail();

        // "a"

        list.addMany([]).before('a');

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });
    });

    describe('#byIndex', () => {
      it('should add multiple nodes starting from given index', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y']).byIndex(1);

        // "a" <-> "x" <-> "y" <-> "b" <-> "c"

        list.addMany(['z']).byIndex(4);

        // "a" <-> "x" <-> "y" <-> "b" <-> "z" <-> "c"

        expect(list.length).toBe(6);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('x');
        expect(list.head.next.next.value).toBe('y');
        expect(list.head.next.next.next.value).toBe('b');
        expect(list.head.next.next.next.next.value).toBe('z');
        expect(list.head.next.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('z');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('y');
        expect(list.tail.previous.previous.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.previous.previous.value).toBe('a');
      });

      it('should add multiple nodes to head if given index is zero', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y']).byIndex(0);

        // "x" <-> "y" <-> "a" <-> "b" <-> "c"

        expect(list.length).toBe(5);
        expect(list.head.value).toBe('x');
        expect(list.head.next.value).toBe('y');
        expect(list.head.next.next.value).toBe('a');
        expect(list.head.next.next.next.value).toBe('b');
        expect(list.head.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.previous.previous.previous.value).toBe('y');
        expect(list.tail.previous.previous.previous.previous.value).toBe('x');
      });

      it('should add multiple nodes to tail if given index more than size', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y']).byIndex(4);

        // "a" <-> "b" <-> "c" <-> "x" <-> "y"

        expect(list.length).toBe(5);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.head.next.next.next.value).toBe('x');
        expect(list.head.next.next.next.next.value).toBe('y');
        expect(list.tail.value).toBe('y');
        expect(list.tail.previous.value).toBe('x');
        expect(list.tail.previous.previous.value).toBe('c');
        expect(list.tail.previous.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.previous.value).toBe('a');
      });

      it('should be able to add multiple nodes at given index counting from right to left', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y']).byIndex(-1);

        // "a" <-> "b" <-> "x" <-> "y" <-> "c"

        expect(list.length).toBe(5);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('x');
        expect(list.head.next.next.next.value).toBe('y');
        expect(list.head.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('y');
        expect(list.tail.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.previous.value).toBe('a');
      });

      it('should add a node to head if given index is less than minus size', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        list.addMany(['x', 'y']).byIndex(-4);

        // "x" <-> "y" <-> "a" <-> "b" <-> "c"

        expect(list.length).toBe(5);
        expect(list.head.value).toBe('x');
        expect(list.head.next.value).toBe('y');
        expect(list.head.next.next.value).toBe('a');
        expect(list.head.next.next.next.value).toBe('b');
        expect(list.head.next.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
        expect(list.tail.previous.previous.previous.value).toBe('y');
        expect(list.tail.previous.previous.previous.previous.value).toBe('x');
      });
    });
  });

  describe('#find', () => {
    it('should return the first node found based on given predicate', () => {
      list.add('a').tail();
      list.add('x').tail();
      list.add('b').tail();
      list.add('x').tail();
      list.add('c').tail();

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const found1 = list.find(node => node.previous && node.previous.value === 'a');

      expect(found1.value).toBe('x');
      expect(found1.previous.value).toBe('a');
      expect(found1.next.value).toBe('b');

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const found2 = list.find(node => node.next && node.next.value === 'c');

      expect(found2.value).toBe('x');
      expect(found2.previous.value).toBe('b');
      expect(found2.next.value).toBe('c');
    });

    it('should return undefined when list is empty', () => {
      const found = list.find(node => node.value === 'x');

      expect(found).toBeUndefined();
    });

    it('should return undefined when predicate finds no match', () => {
      list.add('a').tail();
      list.add('b').tail();
      list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const found = list.find(node => node.value === 'x');

      expect(found).toBeUndefined();
      expect(list.length).toBe(3);
      expect(list.head.value).toBe('a');
      expect(list.head.next.value).toBe('b');
      expect(list.head.next.next.value).toBe('c');
      expect(list.tail.value).toBe('c');
      expect(list.tail.previous.value).toBe('b');
      expect(list.tail.previous.previous.value).toBe('a');
    });
  });

  describe('#findIndex', () => {
    it('should return the index of the first node found based on given predicate', () => {
      list.add('a').tail();
      list.add('x').tail();
      list.add('b').tail();
      list.add('x').tail();
      list.add('c').tail();

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const index1 = list.findIndex(node => node.previous && node.previous.value === 'a');

      expect(index1).toBe(1);

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const index2 = list.findIndex(node => node.next && node.next.value === 'c');

      expect(index2).toBe(3);
    });

    it('should return -1 when list is empty', () => {
      const index = list.findIndex(node => node.value === 'x');

      expect(index).toBe(-1);
    });

    it('should return -1 when no match is found', () => {
      list.add('a').tail();
      list.add('b').tail();
      list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const index = list.findIndex(node => node.value === 'x');

      expect(index).toBe(-1);
      expect(list.length).toBe(3);
      expect(list.head.value).toBe('a');
      expect(list.head.next.value).toBe('b');
      expect(list.head.next.next.value).toBe('c');
      expect(list.tail.value).toBe('c');
      expect(list.tail.previous.value).toBe('b');
      expect(list.tail.previous.previous.value).toBe('a');
    });
  });

  describe('#forEach', () => {
    it('should call given function for each node of the list', () => {
      const a = list.add('a').tail();
      const b = list.add('b').tail();
      const c = list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const spy = jest.fn();
      list.forEach(spy);

      expect(spy.mock.calls).toEqual([
        [a, 0, list],
        [b, 1, list],
        [c, 2, list],
      ]);
    });

    it('should not call given function when list is empty', () => {
      const spy = jest.fn();
      list.forEach(spy);

      expect(spy).not.toHaveBeenCalled();
    });
  });

  describe('#drop', () => {
    describe('#head', () => {
      it('should return undefined when there is no head', () => {
        const dropped = list.drop().head();
        expect(dropped).toBeUndefined();
      });

      it('should remove the node from the head of the list', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.drop().head();

        // "b" <-> "c"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('b');
        expect(list.head.next.value).toBe('c');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.next).toBeUndefined();

        // "b" <-> "c"

        list.drop().head();
        list.drop().head();

        expect(list.length).toBe(0);
        expect(list.head).toBeUndefined();
        expect(list.tail).toBeUndefined();
      });
    });

    describe('#tail', () => {
      it('should return undefined when there is no tail', () => {
        const dropped = list.drop().tail();
        expect(dropped).toBeUndefined();
      });

      it('should remove the node from the tail of the list', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.drop().tail();

        // "a" <-> "b"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('b');
        expect(list.tail.previous.value).toBe('a');
        expect(list.tail.next).toBeUndefined();

        // "a" <-> "b"

        list.drop().tail();
        list.drop().tail();

        expect(list.length).toBe(0);
        expect(list.head).toBeUndefined();
        expect(list.tail).toBeUndefined();
      });
    });

    describe('#byIndex', () => {
      it('should remove the node at given index', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();
        list.add('d').tail();
        list.add('e').tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e"

        list.drop().byIndex(1);

        // "a" <-> "c" <-> "d" <-> "e"

        list.drop().byIndex(-2);

        // "a" <-> "c" <-> "e"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('c');
        expect(list.head.next.next.value).toBe('e');
        expect(list.tail.value).toBe('e');
        expect(list.tail.previous.value).toBe('c');
        expect(list.tail.previous.previous.value).toBe('a');

        // "a" <-> "c" <-> "e"

        list.drop().byIndex(2);
        list.drop().byIndex(1);
        list.drop().byIndex(0);

        expect(list.length).toBe(0);
        expect(list.head).toBeUndefined();
        expect(list.tail).toBeUndefined();
      });

      it('should return undefined when list is empty', () => {
        const dropped = list.drop().byIndex(0);
        expect(dropped).toBeUndefined();
      });

      it('should return undefined when given index does not exist', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        const dropped1 = list.drop().byIndex(4);

        // "a" <-> "b" <-> "c"

        expect(dropped1).toBeUndefined();
        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');

        // "a" <-> "b" <-> "c"

        const dropped2 = list.drop().byIndex(-4);

        // "a" <-> "b" <-> "c"

        expect(dropped2).toBeUndefined();
        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });
    });

    describe('#byValue', () => {
      it('should remove the first node with given value', () => {
        list.add('a').tail();
        list.add('x').tail();
        list.add('b').tail();
        list.add('x').tail();
        list.add('c').tail();

        // "a" <-> "x" <-> "b" <-> "x" <-> "c"

        list.drop().byValue('x');

        // "a" <-> "b" <-> "x" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('x');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('x');
        expect(list.tail.previous.previous.value).toBe('b');
        expect(list.tail.previous.previous.previous.value).toBe('a');

        // "a" <-> "b" <-> "x" <-> "c"

        list.drop().byValue('x');

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.add({ x: 1 }).tail();
        list.add({ x: 2 }).tail();
        list.add({ x: 3 }).tail();

        // {"x":1} <-> {"x":2} <-> {"x":3}

        list.drop().byValue({ x: 2 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":3}

        expect(list.length).toBe(2);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(1);
      });

      it('should return undefined when list is empty', () => {
        const dropped = list.drop().byValue('x');
        expect(dropped).toBeUndefined();
      });

      it('should return undefined when given value is not found', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        const node = list.drop().byValue('x');

        // "a" <-> "b" <-> "c"

        expect(node).toBeUndefined();
        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });
    });

    describe('#byValueAll', () => {
      it('should remove all nodes with given value', () => {
        list.add('a').tail();
        list.add('x').tail();
        list.add('b').tail();
        list.add('x').tail();
        list.add('c').tail();

        // "a" <-> "x" <-> "b" <-> "x" <-> "c"

        const dropped = list.drop().byValueAll('x');

        // "a" <-> "b" <-> "c"

        expect(dropped.length).toBe(2);
        expect(dropped[0].value).toEqual('x');
        expect(dropped[0].previous.value).toEqual('a');
        expect(dropped[0].next.value).toEqual('b');
        expect(dropped[1].value).toEqual('x');
        expect(dropped[1].previous.value).toEqual('b');
        expect(dropped[1].next.value).toEqual('c');

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should be able to receive a custom compareFn', () => {
        list.add({ x: 1 }).tail();
        list.add({ x: 0 }).tail();
        list.add({ x: 2 }).tail();
        list.add({ x: 0 }).tail();
        list.add({ x: 3 }).tail();

        // {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

        const dropped = list.drop().byValueAll({ x: 0 }, (v1: X, v2: X) => v1.x === v2.x);

        // {"x":1} <-> {"x":2} <-> {"x":3}

        expect(dropped.length).toBe(2);
        expect(dropped[0].value.x).toEqual(0);
        expect(dropped[0].previous.value.x).toEqual(1);
        expect(dropped[0].next.value.x).toEqual(2);
        expect(dropped[1].value.x).toEqual(0);
        expect(dropped[1].previous.value.x).toEqual(2);
        expect(dropped[1].next.value.x).toEqual(3);

        expect(list.length).toBe(3);
        expect(list.head.value.x).toBe(1);
        expect(list.head.next.value.x).toBe(2);
        expect(list.head.next.next.value.x).toBe(3);
        expect(list.tail.value.x).toBe(3);
        expect(list.tail.previous.value.x).toBe(2);
        expect(list.tail.previous.previous.value.x).toBe(1);
      });

      it('should return empty array when list is empty', () => {
        const dropped = list.drop().byValueAll('x');
        expect(dropped).toEqual([]);
      });

      it('should return empty array when given value is not found', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        const dropped = list.drop().byValueAll('x');

        // "a" <-> "b" <-> "c"

        expect(dropped).toEqual([]);

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });
    });
  });

  describe('#dropMany', () => {
    describe('#head', () => {
      it('should return empty array when there is no head', () => {
        const dropped = list.dropMany(1).head();
        expect(dropped).toEqual([]);
      });

      it('should remove multiple nodes from the head of the list', () => {
        list.addMany(['a', 'b', 'c', 'd', 'e']).tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e"

        list.dropMany(3).head();

        // "d" <-> "e"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('d');
        expect(list.head.next.value).toBe('e');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('e');
        expect(list.tail.previous.value).toBe('d');
        expect(list.tail.next).toBeUndefined();
      });

      it('should not change the list when count is less than or equal to zero', () => {
        list.add('a').tail();

        // "a"

        list.dropMany(0).head();

        // "a"

        list.dropMany(-1).head();

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });
    });

    describe('#tail', () => {
      it('should return empty array when there is no tail', () => {
        const dropped = list.dropMany(1).tail();
        expect(dropped).toEqual([]);
      });

      it('should remove multiple nodes from the tail of the list', () => {
        list.addMany(['a', 'b', 'c', 'd', 'e']).tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e"

        list.dropMany(3).tail();

        // "a" <-> "b"

        expect(list.length).toBe(2);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('b');
        expect(list.tail.previous.value).toBe('a');
        expect(list.tail.next).toBeUndefined();
      });

      it('should not change the list when count is less than or equal to zero', () => {
        list.add('a').tail();

        // "a"

        list.dropMany(0).tail();

        // "a"

        list.dropMany(-1).tail();

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });
    });

    describe('#byIndex', () => {
      it('should remove multiple nodes starting from given index', () => {
        list.addMany(['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h']).tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e" <-> "f" <-> "g" <-> "h"

        list.dropMany(2).byIndex(1);

        // "a" <-> "d" <-> "e" <-> "f" <-> "g" <-> "h"

        list.dropMany(2).byIndex(-3);

        // "a" <-> "d" <-> "e" <-> "h"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('d');
        expect(list.head.next.next.value).toBe('e');
        expect(list.head.next.next.next.value).toBe('h');
        expect(list.tail.value).toBe('h');
        expect(list.tail.previous.value).toBe('e');
        expect(list.tail.previous.previous.value).toBe('d');
        expect(list.tail.previous.previous.previous.value).toBe('a');

        list.dropMany(4).byIndex(0);

        expect(list.length).toBe(0);
        expect(list.head).toBeUndefined();
        expect(list.tail).toBeUndefined();
      });

      it('should return empty array when list is empty', () => {
        const dropped = list.dropMany(1).byIndex(0);
        expect(dropped).toEqual([]);
      });

      it('should return empty array when given index does not exist', () => {
        list.addMany(['a', 'b', 'c']).tail();

        // "a" <-> "b" <-> "c"

        const dropped = list.dropMany(3).byIndex(4);

        // "a" <-> "b" <-> "c"

        expect(dropped).toEqual([]);
        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should remove from start when given index is less than minus size', () => {
        list.addMany(['a', 'b', 'c', 'd', 'e']).tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e"

        list.dropMany(2).byIndex(-9);

        // "c" <-> "d" <-> "e"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('c');
        expect(list.head.next.value).toBe('d');
        expect(list.head.next.next.value).toBe('e');
        expect(list.tail.value).toBe('e');
        expect(list.tail.previous.value).toBe('d');
        expect(list.tail.previous.previous.value).toBe('c');
      });

      it('should remove from end when given index + count is larger than or equal to size', () => {
        list.addMany(['a', 'b', 'c', 'd', 'e']).tail();

        // "a" <-> "b" <-> "c" <-> "d" <-> "e"

        list.dropMany(2).byIndex(3);

        // "a" <-> "b" <-> "c"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');

        // "a" <-> "b" <-> "c"

        list.dropMany(9).byIndex(1);

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });

      it('should not change the list when count is less than or equal to zero', () => {
        list.add('a').tail();

        // "a"

        list.dropMany(0).byIndex(0);

        // "a"

        list.dropMany(-1).byIndex(0);

        // "a"

        expect(list.length).toBe(1);
        expect(list.head.value).toBe('a');
        expect(list.tail.value).toBe('a');
      });
    });
  });

  describe('#get', () => {
    it('should return node at given index', () => {
      list.add('a').tail();
      list.add('b').tail();
      list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const node = list.get(1);

      expect(node.value).toBe('b');
      expect(node.previous.value).toBe('a');
      expect(node.next.value).toBe('c');
    });

    it('should return undefined when list is empty', () => {
      const node = list.get(1);

      expect(node).toBeUndefined();
    });

    it('should return undefined when predicate finds no match', () => {
      list.add('a').tail();
      list.add('b').tail();
      list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const node1 = list.get(4);

      expect(node1).toBeUndefined();
      expect(list.length).toBe(3);
      expect(list.head.value).toBe('a');
      expect(list.head.next.value).toBe('b');
      expect(list.head.next.next.value).toBe('c');
      expect(list.tail.value).toBe('c');
      expect(list.tail.previous.value).toBe('b');
      expect(list.tail.previous.previous.value).toBe('a');

      // "a" <-> "b" <-> "c"

      const node2 = list.get(-1);

      expect(node2).toBeUndefined();
      expect(list.length).toBe(3);
      expect(list.head.value).toBe('a');
      expect(list.head.next.value).toBe('b');
      expect(list.head.next.next.value).toBe('c');
      expect(list.tail.value).toBe('c');
      expect(list.tail.previous.value).toBe('b');
      expect(list.tail.previous.previous.value).toBe('a');
    });
  });

  describe('#indexOf', () => {
    it('should return the index of the first node found based on given value', () => {
      list.add('a').tail();
      list.add('x').tail();
      list.add('b').tail();
      list.add('x').tail();
      list.add('c').tail();

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const index1 = list.indexOf('x');

      expect(index1).toBe(1);

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      let timesFound = 0;
      const index2 = list.indexOf('x', (v1: string, v2: string) => {
        if (timesFound > 1) return false;

        timesFound += Number(v1 === v2);

        return timesFound > 1;
      });

      expect(index2).toBe(3);
    });

    it('should be able to receive a custom compareFn', () => {
      list.add({ x: 1 }).tail();
      list.add({ x: 2 }).tail();
      list.add({ x: 3 }).tail();

      // {"x":1} <-> {"x":2} <-> {"x":3}

      const index = list.indexOf({ x: 2 }, (v1: X, v2: X) => v1.x === v2.x);

      expect(index).toBe(1);
    });

    it('should return -1 when list is empty', () => {
      const index = list.indexOf('x');

      expect(index).toBe(-1);
    });

    it('should return -1 when no match is found', () => {
      list.add('a').tail();
      list.add('b').tail();
      list.add('c').tail();

      // "a" <-> "b" <-> "c"

      const index = list.indexOf('x');

      expect(index).toBe(-1);
      expect(list.length).toBe(3);
      expect(list.head.value).toBe('a');
      expect(list.head.next.value).toBe('b');
      expect(list.head.next.next.value).toBe('c');
      expect(list.tail.value).toBe('c');
      expect(list.tail.previous.value).toBe('b');
      expect(list.tail.previous.previous.value).toBe('a');
    });
  });

  describe('#toArray', () => {
    it('should return array representation', () => {
      list.addTail('a');
      list.addTail(2);
      list.addTail('c');
      list.addTail({ k: 4, v: 'd' });

      // "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

      const arr = list.toArray();
      expect(arr).toEqual(['a', 2, 'c', { k: 4, v: 'd' }]);
    });

    it('should return empty array when list is empty', () => {
      const arr = list.toArray();
      expect(arr).toEqual([]);
    });
  });

  describe('#toNodeArray', () => {
    it('should return array of nodes', () => {
      list.addTail('a');
      list.addTail(2);
      list.addTail('c');
      list.addTail({ k: 4, v: 'd' });

      // "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

      const arr = list.toNodeArray();

      expect(arr.every(node => node instanceof ListNode)).toBe(true);
      expect(arr.map(node => node.value)).toEqual(['a', 2, 'c', { k: 4, v: 'd' }]);
    });

    it('should return empty array when list is empty', () => {
      const arr = list.toNodeArray();
      expect(arr).toEqual([]);
    });
  });

  describe('#toString', () => {
    it('should return string representation', () => {
      list.addTail('a');
      list.addTail(2);
      list.addTail('c');
      list.addTail({ k: 4, v: 'd' });

      // "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

      const str = list.toString();
      expect(str).toBe('"a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}');
    });

    it('should return empty string when list is empty', () => {
      const str = list.toString();
      expect(str).toBe('');
    });

    it('should be coercible', () => {
      list.addMany(['a', 'b', 'c']).tail();

      // "a" <-> "b" <-> "c"

      expect('' + list).toBe('"a" <-> "b" <-> "c"');
    });

    it('should be able to receive a custom mapperFn', () => {
      list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

      // {"x":1} <-> {"x":2} <-> {"x":3}

      const str = list.toString(value => value.x);
      expect(str).toBe('1 <-> 2 <-> 3');
    });
  });

  it('should be iterable', () => {
    list.addTail('a');
    list.addTail('b');
    list.addTail('c');

    // "a" <-> "b" <-> "c"

    const arr = [];

    for (const value of list) {
      arr.push(value);
    }

    expect(arr).toEqual(['a', 'b', 'c']);
  });
});

interface X {
  [k: string]: any;
}
