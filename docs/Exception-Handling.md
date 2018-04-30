## Exception Handling

ABP provides a built-in infrastructure and offers a standard model to exception handling for a web application.

* Automatically **handles all exceptions** and sends a standard **formatted error message** to the client for an API/AJAX request.
* Automatically hides **internal infrastructure errors** and returns a standard error message.
* Provides configuration option to **localize** exception messages.
* Automatically maps standard exceptions to **HTTP status codes** and provides configuration option to map custom exceptions.

### Automatic Exception Handling

`AbpExceptionFilter` handles an exception if **any of the following conditions** meets;

* Exception is thrown by a **controller action** which returns an **object result** (not a view result).
* The request is an AJAX request (`X-Requested-With` HTTP header value is `XMLHttpRequest`).
* Client explicitly accepts the `application/json` content type (via `accept` HTTP header).

If the exception is handled it's automatically **logged** and a formatted **JSON message** is returned to the client.

#### Error Message Format

Error Message is an instance of the `RemoteServiceErrorResponse` class. The simplest error JSON has a **message** property as shown below:

````json
{
  "error": {
    "message": "This topic is locked and can not add a new message"
  }
}
````

There are **optional fields** those can be filled based on the occurred exception.

##### Error Code

Error **code** is an optional and unique string value for the exception. Thrown `Exception` should implement the `IHasErrorCode` interface to fill this field. Example JSON value:

````json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message"
  }
}
````

Error code can also be used to localize the exception and customize the HTTP status code (see the related sections below).

##### Error Details

Error **details** in an optional field of the JSON error message. Thrown `Exception` should implement the `IHasErrorDetails` interface to fill this field. Example JSON value:

```json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message",
    "details": "A more detailed info about the error..."
  }
}
```

##### Validation Errors

**validationErrors** is a standard field that is filled if the thrown exception implements the `IHasValidationErrors` interface.

````json
{
  "error": {
    "code": "App:010046",
    "message": "Your request is not valid, please correct and try again!",
    "validationErrors": [{
      "message": "Username should be minimum lenght of 3.",
      "members": ["userName"]
    },
    {
      "message": "Password is required",
      "members": ["password"]
    }]
  }
}
````

`AbpValidationException` implements the `IHasValidationErrors` interface and it is automatically thrown by the framework when a request input is not valid. So, usually you don't deal with validation errors except you have a very custom validation logic.

#### Logging

Caught exceptions are automatically logged. 

##### Log Level

Exceptions are logged with the `Error` level by default. Log level can be determined by the exception if it implements the `IHasLogLevel` interface. Example:

````C#
public class MyException : Exception, IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    //...
}
````

##### Self Logging Exceptions

Some exception types may need to write additional logs. They can implement the `IExceptionWithSelfLogging` if needed. Example:

````C#
public class MyException : Exception, IExceptionWithSelfLogging
{
    public void Log(ILogger logger)
    {
        //...log additional info
    }
}
````

> `ILogger.LogException` extension methods is used to write exception logs. You can use the same extension method when needed.

### Business Exceptions

Most of your own exceptions will be business exceptions. The `IBusinessException` interface is used to mark an exception as a business exception.

`BusinessException` implements the `IBusinessException` interface in addition to the `IHasErrorCode`, `IHasErrorDetails` and `IHasLogLevel` interfaces. Default log level is `Warning`.

Usually you have an error code related to a particular business exception. Example:

````C#
throw new BusinessException(QaErrorCodes.CanNotVoteYourOwnAnswer);
````

`QaErrorCodes.CanNotVoteYourOwnAnswer` is just a `const string`. Such an error code format is suggested:

````
<code-namespace>:<error-code>
````

**code-namespace** is a **unique value** specific to your module/application. Example:

````
Volo.Qa:010002
````

`Volo.Qa` is the code-namespace here. code-namespace is then will be used while **localizing** exception messages.

* You can **directly throw** a `BusinessException` or **derive** your own exception types from it when needed.
* All properties are optional for the `BusinessException` class. But you generally set either `ErrorCode` or  `Message` property.

### Exception Localization

One problem with throwing exception is how to localize error messages while sending to clients. ABP offers two models and their variants.

#### User Friendly Exception

If an exception implements the `IUserFriendlyException` interface, then ABP does not change it's `Message` and `Details` properties and directly send to the client.

`UserFriendlyException` class is the built-in implementation of the `IUserFriendlyException` interface. Example usage:

````C#
throw new UserFriendlyException(
    "Username should be unique!"
);
````

In this way, **no need to localization** at all. If you want to localize the message, you can inject and use the standard **string localizer** (see the [localization document](Localization.md)). Example:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage"]);
````

Then define it in the **localization resource** for each language. Example:

````json
{
  "culture": "en",
  "texts": {
    "UserNameShouldBeUniqueMessage": "Username should be unique!"
  }
}
````

String localizer already supports **parameterized messages**. Example:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage", "john"]);
````

Then the localization text can be:

````json
"UserNameShouldBeUniqueMessage": "Username should be unique! '{0}' is already taken!"
````

* The `IUserFriendlyException` interface is derived from the `IBusinessException` and the `UserFriendlyException` class is derived from the `BusinessException` class.

#### Using Error Codes

`UserFriendlyException` is fine, but has a few problems in advanced usages:

* It requires to **inject string localizer** everywhere and always use it while throwing exceptions.
* Some of the cases, it may **not possible** to inject the string localizer (in a static context or in an entity method).

Instead of localizing the message while throwing the exception, you can separate the process using **error codes**.

First, define the **code-namespace** to **localization resource** mapping in the module configuration:

````C#
services.Configure<ExceptionLocalizationOptions>(options =>
{
    options.MapCodeNamespace("Volo.Qa", typeof(QaResource));
});
````

Then any of the exceptions with `Volo.Qa` namespace will be localized using the given localization resource. The localization resource should have an entry with the error code key. Example:

````json
{
  "culture": "en",
  "texts": {
    "Volo.Qa:010002": "You can not vote your own answer!"
  }
}
````

Then a business exception can be thrown with the error code:

````C#
throw new BusinessException(QaDomainErrorCodes.CanNotVoteYourOwnAnswer);
````

* Throwing any exception implementing the `IHasErrorCode` interface behaves the same. So, the error code localization approach is not unique to the `BusinessException` class.
* Defining localized string is not required for an error message. If not defined, ABP sends the default error message to the client. It does not use the `Message` property of the exception! if you want that, use the `UserFriendlyException` (or use an exception type that implements the `IUserFriendlyException` interface).

##### Using Message Parameters

If you have a parameterized error message, then you can set it with the exception's `Data` property. Example:

````C#
throw new BusinessException("App:010046")
{
    Data =
    {
        {"UserName", "john"}
    }
};

````

Fortunately there is a shortcut for it:

````C#
throw new BusinessException("App:010046")
    .WithData("UserName", "john");
````

Then the localized text can contain the`UserName` parameter:

````json
{
  "culture": "en",
  "texts": {
    "App:010046": "Username should be unique. '{UserName}' is already taken!"
  }
}
````

* `WithData` can be chained for more than one parameter (like `.WithData(...).WithData(...)`).

### HTTP Status Code Mapping

ABP tries to automatically determine the most suitable HTTP status code for common exception types by following these rules:

* For the `AbpAuthorizationException`:
  * Returns `401` (unauthorized) if user has not logged in.
  * Returns `403` (forbidden) if user has logged in.
* Returns `400` (bad request) for the `AbpValidationException`.
* Returns `404` (not found) for the `EntityNotFoundException`.
* Returns `403` (forbidden) for the `IBusinessException` (and `IUserFriendlyException` since it extends the `IBusinessException`).
* Returns `501` (not implemented) for the `NotImplementedException`.
* Returns `500` (internal server error) for other exceptions (those are assumed as infrastructure exceptions).

The `IHttpExceptionStatusCodeFinder` is used to automatically determine the HTTP status code. Default implementation is the `DefaultHttpExceptionStatusCodeFinder` class. It can be replaced or extended if needed.

#### Custom Mappings

Automatic HTTP status code determination can be overrided by custom mappings. Example:

````C#
services.Configure<ExceptionHttpStatusCodeOptions>(options =>
{
    options.Map("Volo.Qa:010002", HttpStatusCode.Conflict);
});
````

### Built-In Exceptions

Some exception types are automatically thrown by the framework.

- `AbpAuthorizationException` is thrown if the current user has no permission to perform the requested operation. See authorization document (TODO: link) for more.
- `AbpValidationException` is thrown if the input of the current request is not valid. See validation document (TODO: link) for more.
- `EntityNotFoundException` is thrown if the requested entity is not available. This is mostly thrown by [repositories](Repositories.md).

You can also throw these type of exceptions in your code (while it's rarely needed).