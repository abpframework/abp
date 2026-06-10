# 💥 Top 10 Exception Handling Mistakes in .NET (and How to Actually Fix Them)

Every .NET developer has been there it's 3 AM, production just went down, and the logs are flooding in.  
You open the error trace, only to find… nothing useful. The stack trace starts halfway through a catch block, or worse it's empty. Somewhere, an innocent-looking `throw ex;` or a swallowed background exception has just cost hours of sleep.

Exception handling is one of those things that seems simple on the surface but can quietly undermine an entire system if done wrong. Tiny mistakes like catching `Exception`, forgetting an `await`, or rethrowing incorrectly don't just break code; they break observability. They hide root causes, produce misleading logs, and make even well-architected applications feel unpredictable.

In this article, we'll go through the most common exception handling mistakes developers make in .NET and more importantly, how to fix them. Along the way, you'll see how small choices in your code can mean the difference between a five-minute fix and a full-blown production nightmare.

----------

## 🧨 1. Catching `Exception` (and Everything Else)

**The mistake:**

```csharp
try
{
    // Some operation
}
catch (Exception ex)
{
    // Just to be safe
}

```

**Why it's a problem:**  
Catching the base `Exception` type hides all context including `OutOfMemoryException`, `StackOverflowException`, and other runtime-level issues that you should never handle manually. It also makes debugging painful since you lose the ability to treat specific failures differently.

**The right way:**  
Catch only what you can handle:

```csharp
catch (SqlException ex)
{
    // Handle DB issues
}
catch (IOException ex)
{
    // Handle file issues
}

```

If you really must catch all exceptions (e.g., at a system boundary), **log and rethrow**:

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Unexpected error occurred");
    throw;
}

```

> 💡 **ABP Tip:** In ABP-based applications, you rarely need to catch every exception at the controller or service level.  
> The framework's built-in `AbpExceptionFilter` already handles unexpected exceptions, logs them, and returns standardized JSON responses automatically keeping your controllers clean and consistent.

----------

## 🕳️ 2. Swallowing Exceptions Silently

**The mistake:**

```csharp
try
{
    DoSomething();
}
catch
{
    // ignore
}

```

**Why it's a problem:**  
Silent failures make debugging nearly impossible. You lose stack traces, error context, and sometimes even awareness that something failed at all.

**The right way:**  
Always log or rethrow, unless you have a very specific reason not to:

```csharp
try
{
    _cache.Remove(key);
}
catch (Exception ex)
{
    _logger.LogWarning(ex, "Failed to clear cache key {Key}", key);
}

```

> 💡 **ABP Tip:** Since ABP automatically logs all unhandled exceptions, it's often better to let the framework handle them. Only catch exceptions when you want to enrich logs or add custom business logic before rethrowing.

----------

## 🌀 3. Using `throw ex;` Instead of `throw;`

**The mistake:**

```csharp
catch (Exception ex)
{
    Log(ex);
    throw ex;
}

```

**Why it's a problem:**  
Using `throw ex;` resets the stack trace you lose where the exception actually occurred. This is one of the biggest causes of misleading production logs.

**The right way:**

```csharp
catch (Exception ex)
{
    Log(ex);
    throw; // preserves stack trace
}

```

----------

## ⚙️ 4. Wrapping Everything in Try/Catch

**The mistake:**  
Developers sometimes wrap _every function_ in try/catch “just to be safe.”

**Why it's a problem:**  
This clutters your code and hides the real source of problems. Exception handling should happen at **system boundaries**, not in every method.

**The right way:**  
Handle exceptions at higher levels (e.g., middleware, controllers, background jobs). Let lower layers throw naturally.

> 💡 **ABP Tip:** The ABP Framework provides a top-level exception pipeline via filters and middleware. You can focus purely on your business logic ABP automatically translates unhandled exceptions into standardized API responses.

----------

## 📉 5. Using Exceptions for Control Flow

**The mistake:**

```csharp
try
{
    var user = GetUserById(id);
}
catch (UserNotFoundException)
{
    user = CreateNewUser();
}

```

**Why it's a problem:**  
Exceptions are expensive and should represent _unexpected_ states, not normal control flow.

**The right way:**

```csharp
var user = GetUserByIdOrDefault(id) ?? CreateNewUser();

```

----------

## 🪓 6. Forgetting to Await Async Calls

**The mistake:**

```csharp
try
{
    DoSomethingAsync(); // missing await!
}
catch (Exception ex)
{
    ...
}

```

**Why it's a problem:**  
Without `await`, the exception happens on another thread, outside your `try/catch`. It never gets caught.

**The right way:**

```csharp
try
{
    await DoSomethingAsync();
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error during async operation");
}

```

----------

## 🧵 7. Ignoring Background Task Exceptions

**The mistake:**

```csharp
Task.Run(() => SomeWork());

```

**Why it's a problem:**  
Unobserved task exceptions can crash your process or vanish silently, depending on configuration.

**The right way:**

```csharp
_ = Task.Run(async () =>
{
    try
    {
        await SomeWork();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Background task failed");
    }
});

```

----------

## 📦 8. Throwing Generic Exceptions

**The mistake:**

```csharp
throw new Exception("Something went wrong");

```

**Why it's a problem:**  
Generic exceptions carry no semantic meaning. You can't catch or interpret them specifically later.

**The right way:**  
Use more descriptive types:

```csharp
throw new InvalidOperationException("Order is already processed");

```

> 💡 **ABP Tip:** In ABP applications, you can throw a `BusinessException` or `UserFriendlyException` instead.  
> These support structured data, error codes, localization, and automatic HTTP status mapping:
> 
> ```csharp
> throw new BusinessException("App:010046")
>     .WithData("UserName", "john");
> 
> ```
> 
> This integrates with ABP's localization system, letting your error messages be translated automatically based on the error code.

----------

## 🪞 9. Losing Inner Exceptions

**The mistake:**

```csharp
catch (Exception ex)
{
    throw new CustomException("Failed to process order");
}

```

**Why it's a problem:**  
You lose the inner exception and its stack trace the real reason behind the failure.

**The right way:**

```csharp
catch (Exception ex)
{
    throw new CustomException("Failed to process order", ex);
}

```

> 💡 **ABP Tip:** ABP automatically preserves and logs inner exceptions (for example, inside `BusinessException` chains). You don't need to add boilerplate to capture nested errors just throw them properly.

----------

## 🧭 10. Missing Global Exception Handling

**The mistake:**  
Catching exceptions manually in every controller.

**Why it's a problem:**  
It creates duplicated logic, inconsistent responses, and gaps in logging.

**The right way:**  
Use middleware or a global exception filter:

```csharp
app.UseExceptionHandler("/error");

```

> 💡 **ABP Tip:** ABP already includes a complete global exception system that:
> 
> -   Logs exceptions automatically
>     
> -   Returns a standard `RemoteServiceErrorResponse` JSON object
>     
> -   Maps exceptions to correct HTTP status codes (e.g., 403 for business rules, 404 for entity not found, 400 for validation)
>     
> -   Allows customization through `AbpExceptionHttpStatusCodeOptions`  
>     You can even implement an `ExceptionSubscriber` to react to certain exceptions (e.g., send notifications or trigger audits).
>     

----------

## 🧩 Bonus: Validation Is Not an Exception

**The mistake:**  
Throwing exceptions for predictable user input errors.

**The right way:**  
Use proper validation instead:

```csharp
[Required]
public string UserName { get; set; }

```

> 💡 **ABP Tip:** ABP automatically throws an `AbpValidationException` when DTO validation fails.  
> You don't need to handle this manually ABP formats it into a structured JSON response with `validationErrors`.

----------

## 🧠 Final Thoughts

Exception handling isn't just about preventing crashes it's about making your failures **observable, meaningful, and recoverable**.  
When done right, your logs tell a story: _what happened, where, and why_.  
When done wrong, you're left staring at a 3 AM mystery.

By avoiding these common pitfalls and taking advantage of frameworks like ABP that handle the heavy lifting you'll spend less time chasing ghosts and more time building stable, predictable systems.

