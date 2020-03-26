# Linked List (Doubly)



The core module provides a useful data structure known as a [doubly linked list](https://en.wikipedia.org/wiki/Doubly_linked_list). Briefly, a doubly linked list is a series of records (a.k.a. nodes) which has information on the previous node, the next node, and its own value (or data).



## Getting Started

To create a doubly linked list, all you have to do is to import and create a new instance of it:

```js
import { LinkedList } from '@abp/ng.core';

const list = new LinkedList();
```



The constructor does not get any parameters.



## Usage

### How to Add New Nodes

There are a few methods to create new nodes in a linked list and all of them are separately available as well as revealed from an `add` method.



#### addHead(value: T): ListNode\<T\>

Adds a node with given value as the first node in list:

```js
list.addHead('a');

// "a"

list.addHead('b');

// "b" <-> "a"

list.addHead('c');

// "c" <-> "b" <-> "a"
```



#### addTail(value: T): ListNode\<T\>

Adds a node with given value as the last node in list:

```js
list.addTail('a');

// "a"

list.addTail('b');

// "a" <-> "b"

list.addTail('c');

// "a" <-> "b" <-> "c"
```



#### addAfter(value: T, previousValue: T, compareFn = compare): ListNode\<T\>

Adds a node with given value after the first node that has the previous value:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

list.addAfter('x', 'b');

// "a" <-> "b" <-> "x" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addTail({ x: 1 });
list.addTail({ x: 2 });
list.addTail({ x: 3 });

// {"x":1} <-> {"x":2} <-> {"x":3}

list.addAfter({ x: 0 }, { x: 2 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addBefore(value: T, nextValue: T, compareFn = compare): ListNode\<T\>

Adds a node with given value before the first node that has the next value:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

list.addBefore('x', 'b');

// "a" <-> "x" <-> "b" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addTail({ x: 1 });
list.addTail({ x: 2 });
list.addTail({ x: 3 });

// {"x":1} <-> {"x":2} <-> {"x":3}

list.addBefore({ x: 0 }, { x: 2 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addByIndex(value: T, position: number): ListNode\<T\>

Adds a node with given value at the specified position in the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.addByIndex('x', 2);

// "a" <-> "b" <-> "x" <-> "c"
```



#### add(value: T).head(): ListNode\<T\>

Adds a node with given value as the first node in list:

```js
list.add('a').head();

// "a"

list.add('b').head();

// "b" <-> "a"

list.add('c').head();

// "c" <-> "b" <-> "a"
```



> This is an alternative API for `addHead`. 



#### add(value: T).tail(): ListNode\<T\>

Adds a node with given value as the last node in list:

```js
list.add('a').tail();

// "a"

list.add('b').tail();

// "a" <-> "b"

list.add('c').tail();

// "a" <-> "b" <-> "c"
```



> This is an alternative API for `addTail`. 



#### add(value: T).after(previousValue: T, compareFn = compare): ListNode\<T\>

Adds a node with given value after the first node that has the previous value:

```js
list.add('a').tail();
list.add('b').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "b" <-> "c"

list.add('x').after('b');

// "a" <-> "b" <-> "x" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.add({ x: 1 }).tail();
list.add({ x: 2 }).tail();
list.add({ x: 3 }).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list.add({ x: 0 }).after({ x: 2 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> This is an alternative API for `addAfter`.
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### add(value: T).before(nextValue: T, compareFn = compare): ListNode\<T\>

Adds a node with given value before the first node that has the next value:

```js
list.add('a').tail();
list.add('b').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "b" <-> "c"

list.add('x').before('b');

// "a" <-> "x" <-> "b" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.add({ x: 1 }).tail();
list.add({ x: 2 }).tail();
list.add({ x: 3 }).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list.add({ x: 0 }).before({ x: 2 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}
```



> This is an alternative API for `addBefore`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### add(value: T).byIndex(position: number): ListNode\<T\>

Adds a node with given value at the specified position in the list:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.add('x').byIndex(2);

// "a" <-> "b" <-> "x" <-> "c"
```



> This is an alternative API for `addByIndex`. 



### How to Remove Nodes

There are a few methods to remove nodes from a linked list and all of them are separately available as well as revealed from a `drop` method.



#### dropHead(): ListNode\<T\> | undefined

Removes the first node from the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.dropHead();

// "b" <-> "c"
```



#### dropTail(): ListNode\<T\> | undefined

Removes the last node from the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.dropTail();

// "a" <-> "b"
```



#### dropByIndex(position: number): ListNode\<T\> | undefined

Removes the node with the specified position from the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.dropByIndex(1);

// "a" <-> "c"
```



#### dropByValue(value: T, compareFn = compare): ListNode\<T\> | undefined

Removes the first node with given value from the list:

```js
list.addTail('a');
list.addTail('x');
list.addTail('b');
list.addTail('x');
list.addTail('c');

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addTail({ x: 1 });
list.addTail({ x: 0 });
list.addTail({ x: 2 });
list.addTail({ x: 0 });
list.addTail({ x: 3 });

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue({ x: 0 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### dropByValueAll(value: T, compareFn = compare): ListNode\<T\>\[\]

Removes all nodes with given value from the list:

```js
list.addTail('a');
list.addTail('x');
list.addTail('b');
list.addTail('x');
list.addTail('c');

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValueAll('x');

// "a" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addTail({ x: 1 });
list.addTail({ x: 0 });
list.addTail({ x: 2 });
list.addTail({ x: 0 });
list.addTail({ x: 3 });

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue({ x: 0 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### drop().head(): ListNode\<T\> | undefined

Removes the first node in list:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.drop().head();

// "b" <-> "c"
```



> This is an alternative API for `dropHead`. 



#### drop().tail(): ListNode\<T\> | undefined

Removes the last node in list:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.drop().tail();

// "a" <-> "b"
```



> This is an alternative API for `dropTail`. 



#### drop().byIndex(position: number): ListNode\<T\> | undefined

Removes the node with the specified position from the list:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.drop().byIndex(1);

// "a" <-> "c"
```



> This is an alternative API for `dropByIndex`. 



#### drop().byValue(value: T, compareFn = compare): ListNode\<T\> | undefined

Removes the first node with given value from the list:

```js
list.add('a').tail();
list.add('x').tail();
list.add('b').tail();
list.add('x').tail();
list.add('c').tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.add({ x: 1 }).tail();
list.add({ x: 0 }).tail();
list.add({ x: 2 }).tail();
list.add({ x: 0 }).tail();
list.add({ x: 3 }).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.drop().byValue({ x: 0 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> This is an alternative API for `dropByValue`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### drop().byValueAll(value: T, compareFn = compare): ListNode\<T\>\[\]

Removes all nodes with given value from the list:

```js
list.add('a').tail();
list.add('x').tail();
list.add('b').tail();
list.add('x').tail();
list.add('c').tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValueAll('x');

// "a" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.add({ x: 1 }).tail();
list.add({ x: 0 }).tail();
list.add({ x: 2 }).tail();
list.add({ x: 0 }).tail();
list.add({ x: 3 }).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.drop().byValueAll({ x: 0 }, (v1, v2) => v1.x === v2.x);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> This is an alternative API for `dropByValueAll`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



### How to Find Nodes

There are a few methods to find specific nodes in a linked list.



#### find(predicate: ListIteratorFunction\<T\>): ListNode\<T\> | undefined

Finds the first node from the list that matches the given predicate:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

const found = list.find(node => node.value === 'b');

/*
found.value === "b"
found.previous.value === "a"
found.next.value === "b"
*/
```



#### findIndex(predicate: ListIteratorFunction\<T\>): number

Finds the position of the first node from the list that matches the given predicate:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

const i0 = list.findIndex(node => node.next && node.next.value === 'b');
const i1 = list.findIndex(node => node.value === 'b');
const i2 = list.findIndex(node => node.previous && node.previous.value === 'b');
const i3 = list.findIndex(node => node.value === 'x');

/*
i0 === 0
i1 === 1
i2 === 2
i3 === -1
*/
```



#### get(position: number): ListNode\<T\> | undefined

Finds and returns the node with specific position in the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

const found = list.get(1);

/*
found.value === "b"
found.previous.value === "a"
found.next.value === "c"
*/
```



#### indexOf(value: T, compareFn = compare): number

Finds the position of the first node from the list that has the given value:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

const i0 = list.indexOf('a');
const i1 = list.indexOf('b');
const i2 = list.indexOf('c');
const i3 = list.indexOf('x');

/*
i0 === 0
i1 === 1
i2 === 3
i3 === -1
*/
```



You may pass a custom compare function to detect the searched value:

```js
list.addTail({ x: 1 });
list.addTail({ x: 0 });
list.addTail({ x: 2 });
list.addTail({ x: 0 });
list.addTail({ x: 3 });

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

const i0 = indexOf({ x: 1 }, (v1, v2) => v1.x === v2.x);
const i1 = indexOf({ x: 2 }, (v1, v2) => v1.x === v2.x);
const i2 = indexOf({ x: 3 }, (v1, v2) => v1.x === v2.x);
const i3 = indexOf({ x: 0 }, (v1, v2) => v1.x === v2.x);
const i4 = indexOf({ x: 4 }, (v1, v2) => v1.x === v2.x);

/*
i0 === 0
i1 === 2
i2 === 4
i3 === 1
i4 === -1
*/
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



### How to Check All Nodes

There are a few ways to iterate over or display a linked list.



#### forEach(callback: ListIteratorFunction\<T\>): void

Runs a callback function on all nodes in a linked list from head to tail:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.forEach((node, index) => console.log(node.value + index));

// 'a0'
// 'b1'
// 'c2'
```



#### \*\[Symbol.iterator\]\(\)

A linked list is iterable. In other words, you may use methods like `for...of` on it.

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

for(const node of list) {
  console.log(node.value);
}

// 'a'
// 'b'
// 'c'
```



#### toArray(): T[]

Converts a linked list to an array:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

const arr = list.toArray();

/*
arr === ['a', 'b', 'c']
*/
```



#### toString(): string

Converts a linked list to a string representation of nodes and their relations:

```js
list.addTail('a');
list.addTail(2);
list.addTail('c');
list.addTail({ k: 4, v: 'd' });

// "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

const str = list.toString();

/*
str === '"a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}'
*/
```



