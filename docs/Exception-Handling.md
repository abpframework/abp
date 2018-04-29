## Exception Handling

ABP provides a built-in infrastructure and offers a standard model to exception handling for a web application.

* Automatically **handles all exceptions** and sends a standard **formatted error message** to the client for an API/AJAX request.
* Automatically hides **internal infrastructure errors**.
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
      "message": "Username should be unique. 'john' is taken by another user!",
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



TODO:

Exception Interfaces (IHasLogLevel, ICanLogDetails, IHasErrorCode... etc.)

Pre-Defined Base Exception Classes

Standard Exception classes (AbpValidationException)

Exception Localization

HTTP Status Code Mapping