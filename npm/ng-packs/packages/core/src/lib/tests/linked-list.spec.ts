import { LinkedList } from '../utils/linked-list';

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
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('a');
        expect(list.tail.previous.value).toBe('b');
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
        expect(list.head.previous).toBeUndefined();
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.next).toBeUndefined();
      });
    });

    describe('#after', () => {
      it('should place a node after node with given value', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').after('b');

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
      it('should place a node before node with given value', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        list.add('x').before('b');

        // "a" <-> "x" <-> "b" <-> "c"

        expect(list.length).toBe(4);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('x');
        expect(list.head.next.next.value).toBe('b');
        expect(list.head.next.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('x');
        expect(list.tail.previous.previous.previous.value).toBe('a');
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
      it('should place a node at given index', () => {
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

      const node1 = list.find(node => node.previous && node.previous.value === 'a');

      expect(node1.value).toBe('x');
      expect(node1.previous.value).toBe('a');
      expect(node1.next.value).toBe('b');

      // "a" <-> "x" <-> "b" <-> "x" <-> "c"

      const node2 = list.find(node => node.next && node.next.value === 'c');

      expect(node2.value).toBe('x');
      expect(node2.previous.value).toBe('b');
      expect(node2.next.value).toBe('c');
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
        expect(list.drop().head()).toBeUndefined();
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
      });
    });

    describe('#head', () => {
      it('should return undefined when there is no tail', () => {
        expect(list.drop().tail()).toBeUndefined();
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

        list.drop().byIndex(2);

        // "a" <-> "c" <-> "e"

        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('c');
        expect(list.head.next.next.value).toBe('e');
        expect(list.tail.value).toBe('e');
        expect(list.tail.previous.value).toBe('c');
        expect(list.tail.previous.previous.value).toBe('a');
      });

      it('should return undefined when list is empty', () => {
        const node = list.drop().byIndex(0);
        expect(node).toBeUndefined();
      });

      it('should return undefined when given index does not exist', () => {
        list.add('a').tail();
        list.add('b').tail();
        list.add('c').tail();

        // "a" <-> "b" <-> "c"

        const node1 = list.drop().byIndex(4);

        // "a" <-> "b" <-> "c"

        expect(node1).toBeUndefined();
        expect(list.length).toBe(3);
        expect(list.head.value).toBe('a');
        expect(list.head.next.value).toBe('b');
        expect(list.head.next.next.value).toBe('c');
        expect(list.tail.value).toBe('c');
        expect(list.tail.previous.value).toBe('b');
        expect(list.tail.previous.previous.value).toBe('a');

        // "a" <-> "b" <-> "c"

        const node2 = list.drop().byIndex(-1);

        // "a" <-> "b" <-> "c"

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
        const node = list.drop().byValue('x');
        expect(node).toBeUndefined();
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
