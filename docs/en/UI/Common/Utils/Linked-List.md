# Linked List (Doubly)



The @abp/utils package provides a useful data structure known as a [doubly linked list](https://en.wikipedia.org/wiki/Doubly_linked_list). It is availabe in both Angular (via an import) and MVC (via `abp.utils.common` global object).

Briefly, a doubly linked list is a series of records (a.k.a. nodes) which has information on the previous node, the next node, and its own value (or data).



## Getting Started

To create a doubly linked list, all you have to do is to create a new instance of it:

In Angular:

```js
import { LinkedList } from '@abp/utils';

const list = new LinkedList();
```

In MVC:

```js
var list = new abp.utils.common.LinkedList();
```


The constructor does not get any parameters.



## Usage

### How to Add New Nodes

There are several methods to create new nodes in a linked list and all of them are separately available as well as revealed by `add` and `addMany` methods.



#### addHead(value)

```js
addHead(value: T): ListNode<T>
```

Adds a node with given value as the first node in list:

```js
list.addHead('a');

// "a"

list.addHead('b');

// "b" <-> "a"

list.addHead('c');

// "c" <-> "b" <-> "a"
```



#### addManyHead(values)

```js
addManyHead(values: T[]): ListNode<T>[]
```

Adds multiple nodes with given values as the first nodes in list:

```js
list.addManyHead(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.addManyHead(['x', 'y', 'z']);

// "x" <-> "y" <-> "z" <-> "a" <-> "b" <-> "c"
```



#### addTail(value)

```js
addTail(value: T): ListNode<T>
```

Adds a node with given value as the last node in list:

```js
list.addTail('a');

// "a"

list.addTail('b');

// "a" <-> "b"

list.addTail('c');

// "a" <-> "b" <-> "c"
```



#### addManyTail(values)

```js
addManyTail(values: T[]): ListNode<T>[]
```

Adds multiple nodes with given values as the last nodes in list:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.addManyTail(['x', 'y', 'z']);

// "a" <-> "b" <-> "c" <-> "x" <-> "y" <-> "z"
```



#### addAfter(value, previousValue [, compareFn])

```js
addAfter(value: T, previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

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

list.addAfter(
  { x: 0 },
  2,
  (value, searchedValue) => value.x === searchedValue
);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addManyAfter(values, previousValue [, compareFn])

```js
addManyAfter(values: T[], previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Adds multiple nodes with given values after the first node that has the previous value:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

list.addManyAfter(['x', 'y'], 'b');

// "a" <-> "b" <-> "x" <-> "y" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addManyTail([{ x: 1 },{ x: 2 },{ x: 3 }]);

// {"x":1} <-> {"x":2} <-> {"x":3}

list.addManyAfter(
  [{ x: 4 }, { x: 5 }],
  2,
  (value, searchedValue) => value.x === searchedValue
);

// {"x":1} <-> {"x":2} <-> {"x":4} <-> {"x":5} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addBefore(value, nextValue [, compareFn])

```js
addBefore(value: T, nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

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

list.addBefore(
  { x: 0 },
  2,
  (value, searchedValue) => value.x === searchedValue
);

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addManyBefore(values, nextValue [, compareFn])

```js
addManyBefore(values: T[], nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Adds multiple nodes with given values before the first node that has the next value:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

list.addManyBefore(['x', 'y'], 'b');

// "a" <-> "x" <-> "y" <-> "b" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addManyTail([{ x: 1 },{ x: 2 },{ x: 3 }]);

// {"x":1} <-> {"x":2} <-> {"x":3}

list.addManyBefore(
  [{ x: 4 }, { x: 5 }],
  2,
  (value, searchedValue) => value.x === searchedValue
);

// {"x":1} <-> {"x":4} <-> {"x":5} <-> {"x":2} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addByIndex(value, position)

```js
addByIndex(value: T, position: number): ListNode<T>
```

Adds a node with given value at the specified position in the list:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.addByIndex('x', 2);

// "a" <-> "b" <-> "x" <-> "c"
```



It works with negative index too:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.addByIndex('x', -1);

// "a" <-> "b" <-> "x" <-> "c"
```



#### addManyByIndex(values, position)

```js
addManyByIndex(values: T[], position: number): ListNode<T>[]
```

Adds multiple nodes with given values at the specified position in the list:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.addManyByIndex(['x', 'y'], 2);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```



It works with negative index too:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.addManyByIndex(['x', 'y'], -1);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```



#### add(value).head()

```js
add(value: T).head(): ListNode<T>
```

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



#### add(value).tail()

```js
add(value: T).tail(): ListNode<T>
```

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



#### add(value).after(previousValue [, compareFn])

```js
add(value: T).after(previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

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

list
  .add({ x: 0 })
  .after(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> This is an alternative API for `addAfter`.
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### add(value).before(nextValue [, compareFn])

```js
add(value: T).before(nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

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

list
  .add({ x: 0 })
  .before(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":3}
```



> This is an alternative API for `addBefore`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### add(value).byIndex(position)

```js
add(value: T).byIndex(position: number): ListNode<T>
```

Adds a node with given value at the specified position in the list:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.add('x').byIndex(2);

// "a" <-> "b" <-> "x" <-> "c"
```



It works with negative index too:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.add('x').byIndex(-1);

// "a" <-> "b" <-> "x" <-> "c"
```



> This is an alternative API for `addByIndex`. 



#### addMany(values).head()

```js
addMany(values: T[]).head(): ListNode<T>[]
```

Adds multiple nodes with given values as the first nodes in list:

```js
list.addMany(['a', 'b', 'c']).head();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y', 'z']).head();

// "x" <-> "y" <-> "z" <-> "a" <-> "b" <-> "c"
```



> This is an alternative API for `addManyHead`. 



#### addMany(values).tail()

```js
addMany(values: T[]).tail(): ListNode<T>[]
```

Adds multiple nodes with given values as the last nodes in list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y', 'z']).tail();

// "a" <-> "b" <-> "c" <-> "x" <-> "y" <-> "z"
```



> This is an alternative API for `addManyTail`. 



#### addMany(values).after(previousValue [, compareFn])

```js
addMany(values: T[]).after(previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Adds multiple nodes with given values after the first node that has the previous value:

```js
list.addMany(['a', 'b', 'b', 'c']).tail();

// "a" <-> "b" <-> "b" <-> "c"

list.addMany(['x', 'y']).after('b');

// "a" <-> "b" <-> "x" <-> "y" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list
  .addMany([{ x: 4 }, { x: 5 }])
  .after(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":4} <-> {"x":5} <-> {"x":3}
```



> This is an alternative API for `addManyAfter`.
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addMany(values).before(nextValue [, compareFn])

```js
addMany(values: T[]).before(nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Adds multiple nodes with given values before the first node that has the next value:

```js
list.addMany(['a', 'b', 'b', 'c']).tail();

// "a" <-> "b" <-> "b" <-> "c"

list.addMany(['x', 'y']).before('b');

// "a" <-> "x" <-> "y" <-> "b" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list
  .addMany([{ x: 4 }, { x: 5 }])
  .before(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":4} <-> {"x":5} <-> {"x":2} <-> {"x":3}
```



> This is an alternative API for `addManyBefore`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### addMany(values).byIndex(position)

```js
addMany(values: T[]).byIndex(position: number): ListNode<T>[]
```

Adds multiple nodes with given values at the specified position in the list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y']).byIndex(2);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```



It works with negative index too:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y']).byIndex(-1);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```



> This is an alternative API for `addManyByIndex`. 



### How to Remove Nodes

There are a few methods to remove nodes from a linked list and all of them are separately available as well as revealed from a `drop` method.



#### dropHead()

```js
dropHead(): ListNode<T> | undefined
```

Removes the first node from the list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropHead();

// "b" <-> "c"
```



#### dropManyHead(count)

```js
dropManyHead(count: number): ListNode<T>[]
```

Removes the first nodes from the list based on given count:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropManyHead(2);

// "c"
```



#### dropTail()

```js
dropTail(): ListNode<T> | undefined
```

Removes the last node from the list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropTail();

// "a" <-> "b"
```



#### dropManyTail(count)

```js
dropManyTail(count: number): ListNode<T>[]
```

Removes the last nodes from the list based on given count:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropManyTail(2);

// "a"
```



#### dropByIndex(position)

```js
dropByIndex(position: number): ListNode<T> | undefined
```

Removes the node with the specified position from the list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropByIndex(1);

// "a" <-> "c"
```



It works with negative index too:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropByIndex(-2);

// "a" <-> "c"
```



#### dropManyByIndex(count, position)

```js
dropManyByIndex(count: number, position: number): ListNode<T>[]
```

Removes the nodes starting from the specified position from the list based on given count:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropManyByIndex(2, 1);

// "a" <-> "d"
```



It works with negative index too:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropManyByIndex(2, -2);

// "a" <-> "d"
```



#### dropByValue(value [, compareFn])

```js
dropByValue(value: T, compareFn?: ListComparisonFn<T>): ListNode<T> | undefined
```

Removes the first node with given value from the list:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### dropByValueAll(value [, compareFn])

```js
dropByValueAll(value: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Removes all nodes with given value from the list:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValueAll('x');

// "a" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### drop().head()

```js
drop().head(): ListNode<T> | undefined
```

Removes the first node in list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().head();

// "b" <-> "c"
```



> This is an alternative API for `dropHead`. 



#### drop().tail()

```js
drop().tail(): ListNode<T> | undefined
```

Removes the last node in list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().tail();

// "a" <-> "b"
```



> This is an alternative API for `dropTail`. 



#### drop().byIndex(position)

```js
drop().byIndex(position: number): ListNode<T> | undefined
```

Removes the node with the specified position from the list:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().byIndex(1);

// "a" <-> "c"
```



It works with negative index too:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().byIndex(-2);

// "a" <-> "c"
```



> This is an alternative API for `dropByIndex`. 



#### drop().byValue(value [, compareFn])

```js
drop().byValue(value: T, compareFn?: ListComparisonFn<T>): ListNode<T> | undefined
```

Removes the first node with given value from the list:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list
  .drop()
  .byValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> This is an alternative API for `dropByValue`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### drop().byValueAll(value [, compareFn])

```js
drop().byValueAll(value: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

Removes all nodes with given value from the list:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValueAll('x');

// "a" <-> "b" <-> "c"
```



You may pass a custom compare function to detect the searched value:

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list
  .drop()
  .byValueAll(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> This is an alternative API for `dropByValueAll`. 
>
> The default compare function checks deep equality, so you will rarely need to pass that parameter.



#### dropMany(count).head()

```js
dropMany(count: number).head(): ListNode<T>[]
```

Removes the first nodes from the list based on given count:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropMany(2).head();

// "c"
```



> This is an alternative API for `dropManyHead`. 



#### dropMany(count).tail()

```js
dropMany(count: number).tail(): ListNode<T>[]
```

Removes the last nodes from the list based on given count:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropMany(2).tail();

// "a"
```



> This is an alternative API for `dropManyTail`.



#### dropMany(count).byIndex(position)

```js
dropMany(count: number).byIndex(position: number): ListNode<T>[]
```

Removes the nodes starting from the specified position from the list based on given count:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropMany(2).byIndex(1);

// "a" <-> "d"
```



It works with negative index too:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropMany(2).byIndex(-2);

// "a" <-> "d"
```



> This is an alternative API for `dropManyByIndex`.



### How to Find Nodes

There are a few methods to find specific nodes in a linked list.



#### head

```js
head: ListNode<T> | undefined;
```

Refers to the first node in the list.



#### tail

```js
tail: ListNode<T> | undefined;
```

Refers to the last node in the list.



#### length

```js
length: number;
```

Is the total number of nodes in the list.



#### find(predicate)

```js
find(predicate: ListIteratorFn<T>): ListNode<T> | undefined
```

Finds the first node from the list that matches the given predicate:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

var found = list.find(node => node.value === 'b');

/*
found.value === "b"
found.previous.value === "a"
found.next.value === "b"
*/
```



#### findIndex(predicate)

```js
findIndex(predicate: ListIteratorFn<T>): number
```

Finds the position of the first node from the list that matches the given predicate:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

var i0 = list.findIndex(node => node.next && node.next.value === 'b');
var i1 = list.findIndex(node => node.value === 'b');
var i2 = list.findIndex(node => node.previous && node.previous.value === 'b');
var i3 = list.findIndex(node => node.value === 'x');

/*
i0 === 0
i1 === 1
i2 === 2
i3 === -1
*/
```



#### get(position)

```js
get(position: number): ListNode<T> | undefined
```

Finds and returns the node with specific position in the list:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

var found = list.get(1);

/*
found.value === "b"
found.previous.value === "a"
found.next.value === "c"
*/
```



#### indexOf(value [, compareFn])

```js
indexOf(value: T, compareFn?: ListComparisonFn<T>): number
```

Finds the position of the first node from the list that has the given value:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

var i0 = list.indexOf('a');
var i1 = list.indexOf('b');
var i2 = list.indexOf('c');
var i3 = list.indexOf('x');

/*
i0 === 0
i1 === 1
i2 === 3
i3 === -1
*/
```



You may pass a custom compare function to detect the searched value:

```js
list.addManyTail([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]);

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

var i0 = indexOf(1, (value, searchedValue) => value.x === searchedValue);
var i1 = indexOf(2, (value, searchedValue) => value.x === searchedValue);
var i2 = indexOf(3, (value, searchedValue) => value.x === searchedValue);
var i3 = indexOf(0, (value, searchedValue) => value.x === searchedValue);
var i4 = indexOf(4, (value, searchedValue) => value.x === searchedValue);

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



#### forEach(iteratorFn)

```js
forEach(iteratorFn: ListIteratorFn<T>): void
```

Runs a function on all nodes in a linked list from head to tail:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.forEach((node, index) => console.log(node.value + index));

// 'a0'
// 'b1'
// 'c2'
```


#### \*\[Symbol.iterator\]\(\)

A linked list is iterable. In other words, you may use methods like `for...of` on it.

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

for(const node of list) { /* ES6 for...of statement */
  console.log(node.value);
}

// 'a'
// 'b'
// 'c'
```



#### toArray()

```js
toArray(): T[]
```

Converts a linked list to an array of values:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

var arr = list.toArray();

/*
arr === ['a', 'b', 'c']
*/
```



#### toNodeArray()

```js
toNodeArray(): ListNode<T>[]
```

Converts a linked list to an array of nodes:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

var arr = list.toNodeArray();

/*
arr[0].value === 'a'
arr[1].value === 'a'
arr[2].value === 'a'
*/
```



#### toString([mapperFn])

```js
toString(mapperFn: ListMapperFn<T> = JSON.stringify): string
```

Converts a linked list to a string representation of nodes and their relations:

```js
list.addManyTail(['a', 2, 'c', { k: 4, v: 'd' }]);

// "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

var str = list.toString();

/*
str === '"a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}'
*/
```



You may pass a custom mapper function to map values before stringifying them:

```js
list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }, { x: 4 }, { x: 5 }]).tail();

// {"x":1} <-> {"x":2} <-> {"x":3} <-> {"x":4} <-> {"x":5}

var str = list.toString(value => value.x);

/*
str === '1 <-> 2 <-> 3 <-> 4 <-> 5'
*/
```






## API

### Classes

#### LinkedList

```js
export class LinkedList<T = any> {

  // properties and methods are explained above

}
```



#### ListNode

```js
export class ListNode<T = any> {
  next: ListNode | undefined;

  previous: ListNode | undefined;
  
  constructor(public readonly value: T) {}
}
```

`ListNode` is the node that is being stored in the `LinkedList` for every record.

- `value` is the value stored in the node and is passed through the constructor.
- `next` refers to the next node in the list.
- `previous` refers to the previous node in the list.

```js
list.addManyTail([ 0, 1, 2 ]);

console.log(
  list.head.value,                              // 0
  list.head.next.value,                         // 1
  list.head.next.next.value,                    // 2
  list.head.next.next.previous.value,           // 1
  list.head.next.next.previous.previous.value,  // 0
  list.tail.value,                              // 2
  list.tail.previous.value,                     // 1
  list.tail.previous.previous.value,            // 0
  list.tail.previous.previous.next.value,       // 1
  list.tail.previous.previous.next.next.value,  // 2
);
```




### Types

#### ListMapperFn

```js
type ListMapperFn<T = any> = (value: T) => any;
```

This function is used in `toString` method to map the node values before generating a string representation of the list.



#### ListComparisonFn

```js
type ListComparisonFn<T = any> = (nodeValue: T, comparedValue: any) => boolean;
```

This function is used while adding, dropping, ang finding nodes based on a comparison value.



#### ListIteratorFn

```js
type ListIteratorFn<T = any, R = boolean> = (
  node: ListNode<T>,
  index?: number,
  list?: LinkedList,
) => R;
```

This function is used while iterating over the list either to do something with each node or to find a node.
