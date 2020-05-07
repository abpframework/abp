# 链表 (双向)

@abp/utils包提供了称为[双链表](https://en.wikipedia.org/wiki/Doubly_linked_list)的实用数据结构. 简而言之双向链表是一系列记录(又称节点),这些记录具有上一个节点,下一个节点及其自身值(或数据)的信息.

## 入门

要创建一个双向链表,你需要做的就是导入和创建它的一个新的实例:

```js
import { LinkedList } from '@abp/utils';

var list = new LinkedList();
```

MVC:

```js
var list = new abp.utils.common.LinkedList();
```

构造函数没有任何参数.

## 用法

### 如何添加新节点

有几种方法可以在链表中创建新节点,这些方法都可以单独使用,也可以通过 `add` 和 `addMany` 方法.


#### addHead(value)

```js
addHead(value: T): ListNode<T>
```

将给定值添加到链表的第一个节点:

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

将给定的多个值添加到链表的第一个节点:

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

将给定值添加到链表的最后一个节点:

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

将给定多个值添加到链表的最后一个节点:

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

添加给定值到previousValue节点后:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

list.addAfter('x', 'b');

// "a" <-> "b" <-> "x" <-> "b" <-> "c"
```


你可以自定义比较器:

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


> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.


#### addManyAfter(values, previousValue [, compareFn])

```js
addManyAfter(values: T[], previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

添加给定的多个值到previousValue节点后:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

list.addManyAfter(['x', 'y'], 'b');

// "a" <-> "b" <-> "x" <-> "y" <-> "b" <-> "c"
```

你可以自定义比较器:

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

> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.


#### addBefore(value, nextValue [, compareFn])

```js
addBefore(value: T, nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

添加给值到previousValue节点前:

```js
list.addTail('a');
list.addTail('b');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "b" <-> "c"

list.addBefore('x', 'b');

// "a" <-> "x" <-> "b" <-> "b" <-> "c"
```

你可以自定义比较器:

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

> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### addManyBefore(values, nextValue [, compareFn])

```js
addManyBefore(values: T[], nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```


添加给定的多个值到previousValue节点前:

```js
list.addManyTail(['a', 'b', 'b', 'c']);

// "a" <-> "b" <-> "b" <-> "c"

list.addManyBefore(['x', 'y'], 'b');

// "a" <-> "x" <-> "y" <-> "b" <-> "b" <-> "c"
```



你可以自定义比较器

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



> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### addByIndex(value, position)

```js
addByIndex(value: T, position: number): ListNode<T>
```

在链表的指定位置添加节点:

```js
list.addTail('a');
list.addTail('b');
list.addTail('c');

// "a" <-> "b" <-> "c"

list.addByIndex('x', 2);

// "a" <-> "b" <-> "x" <-> "c"
```

它也适用于负索引:

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

添加多个节点到链表的指定位置:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.addManyByIndex(['x', 'y'], 2);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```

它也适用于负索引:

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

将添加的节点移动到链表头:

```js
list.add('a').head();

// "a"

list.add('b').head();

// "b" <-> "a"

list.add('c').head();

// "c" <-> "b" <-> "a"
```



> 它是 `addHead` 的替代API. 



#### add(value).tail()

```js
add(value: T).tail(): ListNode<T>
```

将添加的节点移动到链表尾:

```js
list.add('a').tail();

// "a"

list.add('b').tail();

// "a" <-> "b"

list.add('c').tail();

// "a" <-> "b" <-> "c"
```



> 它是 `addTail` 的替代API. 



#### add(value).after(previousValue [, compareFn])

```js
add(value: T).after(previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

将添加的节点移动到指定节点后:

```js
list.add('a').tail();
list.add('b').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "b" <-> "c"

list.add('x').after('b');

// "a" <-> "b" <-> "x" <-> "b" <-> "c"
```



你可以自定义比较器

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



> 它是 `addAfter` 的替代API.
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### add(value).before(nextValue [, compareFn])

```js
add(value: T).before(nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>
```

将添加的节点移动到指定节点前:

```js
list.add('a').tail();
list.add('b').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "b" <-> "c"

list.add('x').before('b');

// "a" <-> "x" <-> "b" <-> "b" <-> "c"
```



你可以自定义比较器

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



> 它是 `addBefore` 的替代API.
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### add(value).byIndex(position)

```js
add(value: T).byIndex(position: number): ListNode<T>
```

将添加的节点移动到链表指定位置:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.add('x').byIndex(2);

// "a" <-> "b" <-> "x" <-> "c"
```



它也适用于负索引:

```js
list.add('a').tail();
list.add('b').tail();
list.add('c').tail();

// "a" <-> "b" <-> "c"

list.add('x').byIndex(-1);

// "a" <-> "b" <-> "x" <-> "c"
```



> 它是 `addByIndex` 的替代API. 



#### addMany(values).head()

```js
addMany(values: T[]).head(): ListNode<T>[]
```

将添加的多个节点移动到链表头:

```js
list.addMany(['a', 'b', 'c']).head();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y', 'z']).head();

// "x" <-> "y" <-> "z" <-> "a" <-> "b" <-> "c"
```



> 它是 `addManyHead` 的替代API. 



#### addMany(values).tail()

```js
addMany(values: T[]).tail(): ListNode<T>[]
```

将添加的多个节点移动到链表尾:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y', 'z']).tail();

// "a" <-> "b" <-> "c" <-> "x" <-> "y" <-> "z"
```



> 它是 `addManyTail` 的替代API. 



#### addMany(values).after(previousValue [, compareFn])

```js
addMany(values: T[]).after(previousValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

将添加的多个节点移动到指定节点后:

```js
list.addMany(['a', 'b', 'b', 'c']).tail();

// "a" <-> "b" <-> "b" <-> "c"

list.addMany(['x', 'y']).after('b');

// "a" <-> "b" <-> "x" <-> "y" <-> "b" <-> "c"
```



你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list
  .addMany([{ x: 4 }, { x: 5 }])
  .after(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":4} <-> {"x":5} <-> {"x":3}
```



> 它是 `addManyAfter` 的替代API.
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### addMany(values).before(nextValue [, compareFn])

```js
addMany(values: T[]).before(nextValue: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

将添加的多个节点移动到指定节点前:

```js
list.addMany(['a', 'b', 'b', 'c']).tail();

// "a" <-> "b" <-> "b" <-> "c"

list.addMany(['x', 'y']).before('b');

// "a" <-> "x" <-> "y" <-> "b" <-> "b" <-> "c"
```



你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 2 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":2} <-> {"x":3}

list
  .addMany([{ x: 4 }, { x: 5 }])
  .before(2, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":4} <-> {"x":5} <-> {"x":2} <-> {"x":3}
```



> 它是 `addManyBefore` 的替代API. 
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### addMany(values).byIndex(position)

```js
addMany(values: T[]).byIndex(position: number): ListNode<T>[]
```

将添加的多个节点移动到链表的指定位置:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y']).byIndex(2);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```


它也适用于负索引:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.addMany(['x', 'y']).byIndex(-1);

// "a" <-> "b" <-> "x" <-> "y" <-> "c"
```



> 它是 `addManyByIndex` 的替代API.



### 如何删除节点

有几种方法可以在链表中删除节点,这些方法都可以单独使用,也可以通过 `drop` 方法.



#### dropHead()

```js
dropHead(): ListNode<T> | undefined
```

删除链表的第一个节点:

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

删除指定数量的链表的头节点:

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

删除链表的最后一个节点:

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

删除指定数量的链表的尾节点:

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

删除链表中给定位置的节点:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropByIndex(1);

// "a" <-> "c"
```


它也适用于负索引:

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

删除链表中给定位置与数量的多个节点:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropManyByIndex(2, 1);

// "a" <-> "d"
```



它也适用于负索引:

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

删除链表中含有给定值的第一个节点:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### dropByValueAll(value [, compareFn])

```js
dropByValueAll(value: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

删除链表中含有给定值的所有节点:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.dropByValueAll('x');

// "a" <-> "b" <-> "c"
```

你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list.dropByValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### drop().head()

```js
drop().head(): ListNode<T> | undefined
```

删除链表的头节点:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().head();

// "b" <-> "c"
```



> 它是 `dropHead` 的替代API. 



#### drop().tail()

```js
drop().tail(): ListNode<T> | undefined
```

删除链表的尾节点:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().tail();

// "a" <-> "b"
```



> 它是 `dropTail` 的替代API.



#### drop().byIndex(position)

```js
drop().byIndex(position: number): ListNode<T> | undefined
```

删除链表指定位置的节点:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().byIndex(1);

// "a" <-> "c"
```



它也适用于负索引:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.drop().byIndex(-2);

// "a" <-> "c"
```



> 它是 `dropByIndex` 的替代API. 



#### drop().byValue(value [, compareFn])

```js
drop().byValue(value: T, compareFn?: ListComparisonFn<T>): ListNode<T> | undefined
```

删除链表中含有给定值的第一个节点:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValue('x');

// "a" <-> "b" <-> "x" <-> "c"
```



你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list
  .drop()
  .byValue(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":0} <-> {"x":3}
```



> 它是 `dropByValue` 的替代API. 
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### drop().byValueAll(value [, compareFn])

```js
drop().byValueAll(value: T, compareFn?: ListComparisonFn<T>): ListNode<T>[]
```

删除链表中含有给定值的所有节点:

```js
list.addMany(['a', 'x', 'b', 'x', 'c']).tail();

// "a" <-> "x" <-> "b" <-> "x" <-> "c"

list.drop().byValueAll('x');

// "a" <-> "b" <-> "c"
```



你可以自定义比较器

```js
list.addMany([{ x: 1 }, { x: 0 }, { x: 2 }, { x: 0 }, { x: 3 }]).tail();

// {"x":1} <-> {"x":0} <-> {"x":2} <-> {"x":0} <-> {"x":3}

list
  .drop()
  .byValueAll(0, (value, searchedValue) => value.x === searchedValue);

// {"x":1} <-> {"x":2} <-> {"x":3}
```



> 它是 `dropByValueAll` 的替代API. 
>
> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



#### dropMany(count).head()

```js
dropMany(count: number).head(): ListNode<T>[]
```

删除链表中指定数量的头节点:

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropMany(2).head();

// "c"
```



> 它是 `dropManyHead` 的替代API. 



#### dropMany(count).tail()

```js
dropMany(count: number).tail(): ListNode<T>[]
```

删除链表中指定数量的尾节点::

```js
list.addMany(['a', 'b', 'c']).tail();

// "a" <-> "b" <-> "c"

list.dropMany(2).tail();

// "a"
```



> 它是 `dropManyTail` 的替代API.



#### dropMany(count).byIndex(position)

```js
dropMany(count: number).byIndex(position: number): ListNode<T>[]
```

删除链表中指定位置和数量的节点:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropMany(2).byIndex(1);

// "a" <-> "d"
```



它也适用于负索引:

```js
list.addMany(['a', 'b', 'c', 'd']).tail();

// "a" <-> "b" <-> "c" <-> "d

list.dropMany(2).byIndex(-2);

// "a" <-> "d"
```



> 它是 `dropManyByIndex` 的替代API.



### 如何查找节点

有几个方法找到链表特定节点.

#### head

```js
head: ListNode<T> | undefined;
```

链表中的第一个节点.

#### tail

```js
tail: ListNode<T> | undefined;
```

链表中的最后一个节点.

#### length

```js
length: number;
```

链表的节点总数.

#### find(predicate)

```js
find(predicate: ListIteratorFunction<T>): ListNode<T> | undefined
```

从链表中找到与给定谓词匹配的第一个节点:

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
findIndex(predicate: ListIteratorFunction<T>): number
```

从链表中找到与给定谓词匹配的第一个节点的位置:

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

查找并返回链表中特定位置的节点:

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

在链表中找到匹配给定值的第一个节点位置:

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



你可以自定义比较器

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



> 默认的比较函数检查深度相等性,因此你几乎不需要传递该参数.



### 如何检查所有节点

有几种方法来遍历或显示一个链表.



#### forEach(iteratorFn)

```js
forEach(iteratorFn: ListIteratorFn<T>): void
```

从头到尾在链表中的所有节点上运行回调函数:

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

list.forEach((node, index) => console.log(node.value + index));

// 'a0'
// 'b1'
// 'c2'
```

#### \*\[Symbol.iterator\]\(\)

链表是可迭代的. 换句话说你可以使用诸如`for ... of`之类的方法.

```js
list.addManyTail(['a', 'b', 'c']);

// "a" <-> "b" <-> "c"

for(var node of list) { /* ES6 for...of statement */
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

转换链表值为数组:

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
toNodeArray(): T[]
```

转换链表节点为数组:

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

#### toString(mapperFn)

```js
toString(mapperFn: ListMapperFn<T> = JSON.stringify): string
```

将链表转换为节点及其关系的字符串表示形式:

```js
list.addManyTail(['a', 2, 'c', { k: 4, v: 'd' }]);

// "a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}

var str = list.toString();

/*
str === '"a" <-> 2 <-> "c" <-> {"k":4,"v":"d"}'
*/
```

你可以在对值进行字符串化之前通过自定义映射器函数来映射值:

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

`ListNode` 是存储在 `LinkedList` 中的每个记录的节点.

- `value` value是存储在节点中的值,通过构造函数传递.
- `next` 引用列表中的下一个节点.
- `previous`引用列表中的上一个节点.

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

该函数在 `toString` 方法中用于在生成列表的字符串形式之前映射节点值.

#### ListComparisonFn

```js
type ListComparisonFn<T = any> = (nodeValue: T, comparedValue: any) => boolean;
```

该函数用于根据比较值添加,删除和查找节点.

#### ListIteratorFn

```js
type ListIteratorFn<T = any, R = boolean> = (
  node: ListNode<T>,
  index?: number,
  list?: LinkedList,
) => R;
```

该函数在遍历列表时使用,可以对每个节点执行某些操作,也可以查找某个节点.