/* tslint:disable:no-non-null-assertion */

import compare from 'just-compare';

export class ListNode<T = any> {
  next: ListNode | undefined;
  previous: ListNode | undefined;
  constructor(public readonly value: T) {}
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

  private attach(
    value: T,
    previousNode: ListNode<T> | undefined,
    nextNode: ListNode<T> | undefined,
  ): ListNode<T> {
    if (!previousNode) return this.addHead(value);

    if (!nextNode) return this.addTail(value);

    const node = new ListNode(value);
    node.previous = previousNode;
    previousNode.next = node;
    node.next = nextNode;
    nextNode.previous = node;

    this.size++;

    return node;
  }

  private attachMany(
    values: T[],
    previousNode: ListNode<T> | undefined,
    nextNode: ListNode<T> | undefined,
  ): ListNode<T>[] {
    if (!values.length) return [];

    if (!previousNode) return this.addManyHead(values);

    if (!nextNode) return this.addManyTail(values);

    const list = new LinkedList<T>();
    list.addManyTail(values);
    list.first!.previous = previousNode;
    previousNode.next = list.first;
    list.last!.next = nextNode;
    nextNode.previous = list.last;

    this.size += values.length;

    return list.toNodeArray();
  }

  private detach(node: ListNode<T>) {
    if (!node.previous) return this.dropHead();

    if (!node.next) return this.dropTail();

    node.previous.next = node.next;
    node.next.previous = node.previous;

    this.size--;

    return node;
  }

  add(value: T) {
    return {
      after: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.addAfter.call(this, value, ...params),
      before: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.addBefore.call(this, value, ...params),
      byIndex: (position: number) => this.addByIndex(value, position),
      head: () => this.addHead(value),
      tail: () => this.addTail(value),
    };
  }

  addMany(values: T[]) {
    return {
      after: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.addManyAfter.call(this, values, ...params),
      before: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.addManyBefore.call(this, values, ...params),
      byIndex: (position: number) => this.addManyByIndex(values, position),
      head: () => this.addManyHead(values),
      tail: () => this.addManyTail(values),
    };
  }

  addAfter(value: T, previousValue: T): ListNode<T>;
  addAfter(value: T, previousValue: any, compareFn: ListComparisonFn<T>): ListNode<T>;
  addAfter(value: T, previousValue: any, compareFn: ListComparisonFn<T> = compare): ListNode<T> {
    const previous = this.find(node => compareFn(node.value, previousValue));

    return previous ? this.attach(value, previous, previous.next) : this.addTail(value);
  }

  addBefore(value: T, nextValue: T): ListNode<T>;
  addBefore(value: T, nextValue: any, compareFn: ListComparisonFn<T>): ListNode<T>;
  addBefore(value: T, nextValue: any, compareFn: ListComparisonFn<T> = compare): ListNode<T> {
    const next = this.find(node => compareFn(node.value, nextValue));

    return next ? this.attach(value, next.previous, next) : this.addHead(value);
  }

  addByIndex(value: T, position: number): ListNode<T> {
    if (position < 0) position += this.size;
    else if (position >= this.size) return this.addTail(value);

    if (position <= 0) return this.addHead(value);

    const next = this.get(position)!;

    return this.attach(value, next.previous, next);
  }

  addHead(value: T): ListNode<T> {
    const node = new ListNode(value);

    node.next = this.first;

    if (this.first) this.first.previous = node;
    else this.last = node;

    this.first = node;
    this.size++;

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

    this.size++;

    return node;
  }

  addManyAfter(values: T[], previousValue: T): ListNode<T>[];
  addManyAfter(values: T[], previousValue: any, compareFn: ListComparisonFn<T>): ListNode<T>[];
  addManyAfter(
    values: T[],
    previousValue: any,
    compareFn: ListComparisonFn<T> = compare,
  ): ListNode<T>[] {
    const previous = this.find(node => compareFn(node.value, previousValue));

    return previous ? this.attachMany(values, previous, previous.next) : this.addManyTail(values);
  }

  addManyBefore(values: T[], nextValue: T): ListNode<T>[];
  addManyBefore(values: T[], nextValue: any, compareFn: ListComparisonFn<T>): ListNode<T>[];
  addManyBefore(
    values: T[],
    nextValue: any,
    compareFn: ListComparisonFn<T> = compare,
  ): ListNode<T>[] {
    const next = this.find(node => compareFn(node.value, nextValue));

    return next ? this.attachMany(values, next.previous, next) : this.addManyHead(values);
  }

  addManyByIndex(values: T[], position: number): ListNode<T>[] {
    if (position < 0) position += this.size;

    if (position <= 0) return this.addManyHead(values);

    if (position >= this.size) return this.addManyTail(values);

    const next = this.get(position)!;

    return this.attachMany(values, next.previous, next);
  }

  addManyHead(values: T[]): ListNode<T>[] {
    return values.reduceRight<ListNode<T>[]>((nodes, value) => {
      nodes.unshift(this.addHead(value));
      return nodes;
    }, []);
  }

  addManyTail(values: T[]): ListNode<T>[] {
    return values.map(value => this.addTail(value));
  }

  drop() {
    return {
      byIndex: (position: number) => this.dropByIndex(position),
      byValue: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.dropByValue.apply(this, params),
      byValueAll: (...params: [T] | [any, ListComparisonFn<T>]) =>
        this.dropByValueAll.apply(this, params),
      head: () => this.dropHead(),
      tail: () => this.dropTail(),
    };
  }

  dropMany(count: number) {
    return {
      byIndex: (position: number) => this.dropManyByIndex(count, position),
      head: () => this.dropManyHead(count),
      tail: () => this.dropManyTail(count),
    };
  }

  dropByIndex(position: number): ListNode<T> | undefined {
    if (position < 0) position += this.size;

    const current = this.get(position);

    return current ? this.detach(current) : undefined;
  }

  dropByValue(value: T): ListNode<T> | undefined;
  dropByValue(value: any, compareFn: ListComparisonFn<T>): ListNode<T> | undefined;
  dropByValue(value: any, compareFn: ListComparisonFn<T> = compare): ListNode<T> | undefined {
    const position = this.findIndex(node => compareFn(node.value, value));

    return position < 0 ? undefined : this.dropByIndex(position);
  }

  dropByValueAll(value: T): ListNode<T>[];
  dropByValueAll(value: any, compareFn: ListComparisonFn<T>): ListNode<T>[];
  dropByValueAll(value: any, compareFn: ListComparisonFn<T> = compare): ListNode<T>[] {
    const dropped: ListNode<T>[] = [];

    for (let current = this.first, position = 0; current; position++, current = current.next) {
      if (compareFn(current.value, value)) {
        dropped.push(this.dropByIndex(position - dropped.length)!);
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

      this.size--;

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

      this.size--;

      return tail;
    }

    return undefined;
  }

  dropManyByIndex(count: number, position: number): ListNode<T>[] {
    if (count <= 0) return [];

    if (position < 0) position = Math.max(position + this.size, 0);
    else if (position >= this.size) return [];

    count = Math.min(count, this.size - position);

    const dropped: ListNode<T>[] = [];

    while (count--) {
      const current = this.get(position);
      dropped.push(this.detach(current!)!);
    }

    return dropped;
  }

  dropManyHead(count: Exclude<number, 0>): ListNode<T>[] {
    if (count <= 0) return [];

    count = Math.min(count, this.size);

    const dropped: ListNode<T>[] = [];

    while (count--) dropped.unshift(this.dropHead()!);

    return dropped;
  }

  dropManyTail(count: Exclude<number, 0>): ListNode<T>[] {
    if (count <= 0) return [];

    count = Math.min(count, this.size);

    const dropped: ListNode<T>[] = [];

    while (count--) dropped.push(this.dropTail()!);

    return dropped;
  }

  find(predicate: ListIteratorFn<T>): ListNode<T> | undefined {
    for (let current = this.first, position = 0; current; position++, current = current.next) {
      if (predicate(current, position, this)) return current;
    }

    return undefined;
  }

  findIndex(predicate: ListIteratorFn<T>): number {
    for (let current = this.first, position = 0; current; position++, current = current.next) {
      if (predicate(current, position, this)) return position;
    }

    return -1;
  }

  forEach<R = boolean>(iteratorFn: ListIteratorFn<T, R>) {
    for (let node = this.first, position = 0; node; position++, node = node.next) {
      iteratorFn(node, position, this);
    }
  }

  get(position: number): ListNode<T> | undefined {
    return this.find((_, index) => position === index);
  }

  indexOf(value: T): number;
  indexOf(value: any, compareFn: ListComparisonFn<T>): number;
  indexOf(value: any, compareFn: ListComparisonFn<T> = compare): number {
    return this.findIndex(node => compareFn(node.value, value));
  }

  toArray(): T[] {
    const array = new Array(this.size);

    this.forEach((node, index) => (array[index!] = node.value));

    return array;
  }

  toNodeArray(): ListNode<T>[] {
    const array = new Array(this.size);

    this.forEach((node, index) => (array[index!] = node));

    return array;
  }

  toString(mapperFn: ListMapperFn<T> = JSON.stringify): string {
    return this.toArray()
      .map(value => mapperFn(value))
      .join(' <-> ');
  }

  // Cannot use Generator type because of ng-packagr
  *[Symbol.iterator](): any {
    for (let node = this.first, position = 0; node; position++, node = node.next) {
      yield node.value;
    }
  }
}

export type ListMapperFn<T = any> = (value: T) => any;

export type ListComparisonFn<T = any> = (value1: T, value2: any) => boolean;

export type ListIteratorFn<T = any, R = boolean> = (
  node: ListNode<T>,
  index?: number,
  list?: LinkedList,
) => R;
