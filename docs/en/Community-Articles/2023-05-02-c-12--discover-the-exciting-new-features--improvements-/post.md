Several exciting new features come with C# 12 and the.NET 8 preview. C# is easier to use and more effective than **previous** versions thanks to changes made by the Microsoft development team.  **In** **this** **post,** **we'll** **take** **a** **quick** **look** **at** some of the **major** changes in C# **12.**



## 1. Improved Support for Interpolated Strings 🧩

With the release of C# 12, you now have the option to incorporate expressions inside interpolated strings. With the help of this functionality, You can now create dynamic values for strings using complicated expressions and techniques.

For instance, you can use code like this:

```
int x = 10;
string message = $"The value of x is {x}, and its square is {x*x}.";
```

This will result in a string value of "*The value of x is 10, and its square is 100.*"

## 2. Primary Constructors 📝


You can now create primary constructors in any class and struct. With primary constructors, developers can add parameters to the class declaration and use these values inside the class body.

Primary constructors were introduced in C# 9 as part of the record positional syntax. C# 12 extends these to all structs and classes.

```csharp
 public class Book(int pageSize, string authorName, IEnumerable<float> prices)
  {
      public Book(int pageSize, string authorName) : this(pageSize, authorName, Enumerable.Empty<float>()) { }
      public int pageSize => pageSize;
      public string authorName { get; set; } = authorName;
      public float totalPrice => prices.Sum();
  }
```

The good part is that you can avoid the pain of declaring private fields and, you know, binding parameter values to those fields in those tedious constructor bodies by using primary constructors instead.

## 3. Improved Lambda Expressions 🐑

Many of us are using Lambda expressions in our daily development to gain the code better readability. 
With this version we see that many enhancements came to lambda expressions make them more effective.  
For example, you can now create more complex expressions within lambda functions using the new `and` , `or` operators.
Besides, lambda expressions can now be transformed into expression trees, simplifying the construction of complex queries and optimizing performance.

## 4. Async Streams 🌐

You may iterate through asynchronous data sources thanks to the new `async` streams feature in C# 12. 
This new iterator `await foreach` gains us to iterate over a set of async data.
See the following code snippet:

```csharp
await foreach (var record in FetchRecordsAsyncData())
{
   Console.WriteLine(record.text);
}
```

This will iterate over the asynchronous data returned by the `GetAsyncData()` method and writes each item's text to the console.

## 5. Target-typed New Expressions 🎯

Target-typed new expressions, a new feature in C# 12, make it simpler to construct new objects. 
You can now declare new objects using the `var` keyword. 
The object's type is inferred from the context.
Especially when dealing with complex types, it gives us better code readability.

An alternative to writing:

```csharp
AcmeClass thatObject = new AcmeClass();
```

You can now write:

```csharp
var thatObject = new AcmeClass();
```



## Conclusion 🎬

As always Microsoft makes developers' life easier with every release of .NET and C#. 
C# 12 comes with many features and improvements. 
You now have new tools at your disposal to create more effective, concise, and durable code.
You may now design more potent and reliable applications with C# thanks to the increased support for interpolated strings, records, lambda expressions, target-typed new expressions, and async streams.



## Start C# 12 with the ABP Framework 📚

ABP Framework offers an opinionated architecture to build enterprise  software solutions with ASP.NET Core best practices on top of the .NET  and the ASP.NET Core platforms. It is also a powerful infrastructure to  help you develop low-effort web-optimized applications. It provides the fundamental web application infrastructure,  production-ready dotnet startup templates, modules, asp.net core ui  themes, tooling, guides and documentation to implement that ASP.NET core architecture properly and automate the details and repetitive work as  much as possible.

If you are starting a new ASP.NET Core project and want a fast website [abp.io](https://abp.io/) now...

🆓 **It's FREE & OPEN-SOURCE!** 🔓


Happy coding 🍿✨


---



> I'm Alper Ebicoglu 🧑🏽‍💻\
> ABP Framework Core Team Member\
> Follow me for the latest news about .NET and software development:\
> 📌 [twitter.com/alperebicoglu](https://twitter.com/alperebicoglu)\
> 📌 [github.com/ebicoglu](https://github.com/ebicoglu)\
> 📌 [linkedin.com/in/ebicoglu](https://www.linkedin.com/in/ebicoglu)\
> 📌 [medium.com/@alperonline](https://medium.com/@alperonline)


