# C# 13 Features

C# 13 is the latest version of C# and it comes with a lot of new features. In this article, we will discuss some of the new features of C# 13.

## `params` collections

With the C# 13, method parameter with `params` keyword isn't limited to be an array. You can now use any collection type that implements `IEnumerable<T>` interface.

Let's see how it can help us in our code.

```csharp
public IEnumerable<int> GetOdds(params IEnumerable<int> numbers)
{
    foreach (var number in numbers)
    {
        if (number % 2 != 0)
        {
            Console.WriteLine(number);
        }
    }
}
```

## New lock object

I'm sure you have used `lock` statement in your code to synchronize access to a shared resource. With C# 13, you can now use a new lock object that is more efficient than the traditional lock object.
The new `Lock` type provides better thread synchronization through its API. When `Lock.EnterScope()` method is called, it returns a struct named `Scope` that contains a `Dispose` method. The `Dispose` method is called when the `Scope` object goes out of scope, which releases the lock. C# `using` statement recognizes the `Dispose` method and calls it automatically like it does with other `IDisposable` objects.

It was something similar before:
```csharp
private object _lock = new();

public void DoSomething()
{
    lock (_lock)
    {
        // Do something
    }
}
```

Now, you can use the new lock object like this:
```csharp
System.Threading.Lock x = new System.Threading.Lock();
public void DoSomething()
{
    using (x.EnterScope())
    {
        // Do something
    }
}
```

## New escape sequence
In C# 13, a new escape sequence `\e` has been introduced to represent the `ESCAPE` character, Unicode `U+001B`. Previously, you had to use `\u001b` or `\x1b` to represent this character. The new `\e` escape sequence simplifies this process and avoids potential issues with hexadecimal digits following `\x1b`.

> You can check [here](https://en.wikipedia.org/wiki/ANSI_escape_code#C0_control_codes) for ANSI escape codes.

## Implicit index access

The implicit "from the end" index operator, `^`, is now allowed in an object initializer expression. 

It was not possible before, but now you can do this:

```csharp
var countdown = new TimerRemaining()
{
    buffer =
    {
        [^1] = 0,
        [^2] = 1,
        [^3] = 2,
        [^4] = 3,
        [^5] = 4,
        [^6] = 5,
        [^7] = 6,
        [^8] = 7,
        [^9] = 8,
        [^10] = 9
    }
};
```

It's a great feature that makes the code more readable and maintainable. Still not a big deal, but it's nice to have it.

## `ref` and `unsafe` in iterators and async methods

In C# 13, the restrictions on using `ref` and `unsafe` constructs in iterators and async methods have been relaxed. Previously, you couldn't declare local `ref` variables or use unsafe contexts in these methods. Now, you can declare ref local variables and use unsafe contexts in async methods and iterators, provided they are not accessed across `await` or `yield` boundaries


This change allows for more expressive and efficient code, especially when working with types like `System.Span<T>` and `System.ReadOnlySpan<T>`. The compiler ensures that these constructs are used safely, and it will notify you if any safety rules are violated.

You can read more about this feature on the [Microsoft Learn page](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-13.0/ref-unsafe-in-iterators-async).



## More partial members

In C# 13, the concept of partial members has been expanded to include partial properties and partial indexers. Previously, only methods could be defined as partial members. This means you can now split the definition of properties and indexers across multiple files, just like you could with methods.

For example, you can declare a partial property in one part of your class and implement it in another part. Here's a simple illustration:

```csharp
public partial class MyClass
{
    // Declaring declaration
    public partial string MyProperty { get; set; }
}

public partial class MyClass
{
    // Implementing declaration
    private string _myProperty;
    public partial string MyProperty
    {
        get => _myProperty;
        set => _myProperty = value;
    }
}
```

This feature allows for better organization and modularization of your code, especially in large projects where different parts of a class might be implemented by different team members.

## Overload resolution priority

What does "Overload resolution priority" section mean in this page?
In C# 13, the OverloadResolutionPriority attribute allows library authors to specify which method overload should be preferred by the compiler when multiple overloads are available. This attribute helps avoid ambiguity and ensures that the most appropriate overload is chosen, even if it might not be the most obvious choice based on traditional overload resolution rules. 

This may be useful in scenarios where you have multiple overloads that are equally valid, but you want to prioritize one over the others. The attribute can be applied to a method or constructor to indicate its priority in the overload resolution process. It can prevent unexpected behavior and make your code more predictable and maintainable.


Let me show with an example:
```csharp
public class Example
{
    // Existing method
    public void Display(string message = "Hello!")
    {
        Console.WriteLine("Message: " + message);
    }

    // New, more efficient method with higher priority
    [OverloadResolutionPriority(1)]
    public void Display(string message = "Hello!", int repeatCount = 3)
    {
        for (int i = 0; i < repeatCount; i++)
        {
            Console.WriteLine("Message: " + message);
        }
    }
}

class Program
{
    static void Main()
    {
        Example example = new Example();

        // Normally, you can't compile this code because of ambiguity:
        example.Display();
    }
}
```

Output:
```
Message: Hello!
Message: Hello!
Message: Hello!
```

## The `field` keyword

n C# 13, the `field` keyword is introduced as a preview feature to simplify property accessors. This keyword allows you to reference the compiler-generated backing `field` directly within a property accessor, eliminating the need to declare an explicit backing `field` in your type declaration.

For example, instead of writing:

```csharp
private int _value;
public int Value
{
    get => _value;
    set => _value = value;
}
```

You can now write:
    
```csharp
public int Value
{
    get => field;
    set => field = value;
}
```

This makes your code cleaner and more concise. However, be cautious if you have a `field` named `field` in your class, as it could cause confusion. You can disambiguate by using `@field` or `this.field`.

Make sure you're using the latest `LangVersion` in your `.csproj` project file to enable this feature.
```xml
<LangVersion>preview</LangVersion>
```