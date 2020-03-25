import compare from 'just-compare';

export class ListNode<T = any> {
  readonly value: T;
  next: ListNode | undefined;
  previous: ListNode | undefined;

  constructor(value: T) {
    this.value = value;
  }
}

export class LinkedList<T = any> {
  private first: ListNode<T> | undefined;
  private last: ListNode<T> | undefined;
  private size = 0;

  get head(): ListNode<T> | undefined {
    return this.first;
  }
  get tail(): ListNode<T> | undefined {
    return this.last;
  }
  get length(): number {
    return this.size;
  }

  private linkWith(value: T, previousNode: ListNode<T>, nextNode: ListNode<T>): ListNode<T> {
    const node = new ListNode(value);

    if (!previousNode) return this.addHead(value);
    if (!nextNode) return this.addTail(value);

    node.previous = previousNode;
    previousNode.next = node;
    node.next = nextNode;
    nextNode.previous = node;

    this.size += 1;

    return node;
  }

  add(value: T) {
    return {
      after: (previousValue: T, compareFn = compare) => {
        return this.addAfter(value, previousValue, compareFn);
      },
      before: (nextValue: T, compareFn = compare) => {
        return this.addBefore(value, nextValue, compareFn);
      },
      byIndex: (position: number): ListNode<T> => {
        return this.addByIndex(value, position);
      },
      head: (): ListNode<T> => {
        return this.addHead(value);
      },
      tail: (): ListNode<T> => {
        return this.addTail(value);
      },
    };
  }

  addAfter(value: T, previousValue: T, compareFn = compare): ListNode<T> {
    const previous = this.find(currentValue => compareFn(currentValue, previousValue));

    return previous ? this.linkWith(value, previous, previous.next) : this.addTail(value);
  }

  addBefore(value: T, nextValue: T, compareFn = compare): ListNode<T> {
    const next = this.find(currentValue => compareFn(currentValue, nextValue));

    return next ? this.linkWith(value, next.previous, next) : this.addHead(value);
  }

  addByIndex(value: T, position: number): ListNode<T> {
    if (position <= 0) return this.addHead(value);
    if (position >= this.size) return this.addTail(value);

    const next = this.get(position)!;

    return this.linkWith(value, next.previous, next);
  }

  addHead(value: T): ListNode<T> {
    const node = new ListNode(value);

    node.next = this.first;

    if (this.first) this.first.previous = node;
    else this.last = node;

    this.first = node;
    this.size += 1;

    return node;
  }

  addTail(value: T): ListNode<T> {
    const node = new ListNode(value);

    if (this.first) {
      node.previous = this.last;
      this.last!.next = node;
      this.last = node;
    } else {
      this.first = node;
      this.last = node;
    }

    this.size += 1;

    return node;
  }

  drop() {
    return {
      byIndex: (position: number) => this.dropByIndex(position),
      byValue: (value: T, compareFn = compare) => this.dropByValue(value, compareFn),
      byValueAll: (value: T, compareFn = compare) => this.dropByValueAll(value, compareFn),
      head: () => this.dropHead(),
      tail: () => this.dropTail(),
    };
  }

  dropByIndex(position: number): ListNode<T> | undefined {
    if (position === 0) return this.dropHead();
    else if (position === this.size - 1) return this.dropTail();

    const current = this.get(position);

    if (current) {
      current.previous!.next = current.next;
      current.next!.previous = current.previous;

      this.size -= 1;

      return current;
    }

    return undefined;
  }

  dropByValue(value: T, compareFn = compare): ListNode<T> | undefined {
    const position = this.findIndex(currentValue => compareFn(currentValue, value));

    if (position < 0) return undefined;

    return this.dropByIndex(position);
  }

  dropByValueAll(value: T, compareFn = compare): ListNode<T>[] {
    const dropped: ListNode<T>[] = [];

    for (let current = this.first, position = 0; current; position += 1, current = current.next) {
      if (compareFn(current.value, value)) {
        dropped.push(this.dropByIndex(position - dropped.length));
      }
    }

    return dropped;
  }

  dropHead(): ListNode<T> | undefined {
    const head = this.first;

    if (head) {
      this.first = head.next;

      if (this.first) this.first.previous = undefined;
      else this.last = undefined;

      this.size -= 1;

      return head;
    }

    return undefined;
  }

  dropTail(): ListNode<T> | undefined {
    const tail = this.last;

    if (tail) {
      this.last = tail.previous;

      if (this.last) this.last.next = undefined;
      else this.first = undefined;

      this.size -= 1;

      return tail;
    }

    return undefined;
  }

  find(predicate: ListIteratorFunction<T>): ListNode<T> | undefined {
    for (let current = this.first, position = 0; current; position += 1, current = current.next) {
      if (predicate(current.value, position, this)) return current;
    }

    return undefined;
  }

  findIndex(predicate: ListIteratorFunction<T>): number {
    for (let current = this.first, position = 0; current; position += 1, current = current.next) {
      if (predicate(current.value, position, this)) return position;
    }

    return -1;
  }

  forEach<R = boolean>(callback: ListIteratorFunction<T, R>) {
    for (let node = this.first, position = 0; node; position += 1, node = node.next) {
      callback(node.value, position, this);
    }
  }

  get(position: number): ListNode<T> | undefined {
    return this.find((_, index) => position === index);
  }

  indexOf(value: T, compareFn = compare): number {
    return this.findIndex(currentValue => compareFn(currentValue, value));
  }

  toArray(): T[] {
    const array = new Array(this.size);

    this.forEach((value, index) => (array[index!] = value));

    return array;
  }

  toString(): string {
    return this.toArray()
      .map(value => JSON.stringify(value))
      .join(' <-> ');
  }

  *[Symbol.iterator]() {
    for (let node = this.first, position = 0; node; position += 1, node = node.next) {
      yield node.value;
    }
  }
}

export type ListIteratorFunction<T = any, R = boolean> = (
  value: T,
  index?: number,
  list?: LinkedList,
) => R;
