## 异常处理

ABP提供了用于处理Web应用程序异常的标准模型.

* 自动 **处理所有异常** .如果是API/AJAX请求,会向客户端返回一个**标准格式化后的错误消息** .
* 自动隐藏 **内部详细错误** 并返回标准错误消息.
* 为异常消息的 **本地化** 提供一种可配置的方式.
* 自动为标准异常设置 **HTTP状态代码** ,并提供可配置选项,以映射自定义异常.

### 自动处理异常

当满足下面**任意一个条件**时,`AbpExceptionFilter` 会处理此异常:

* 当**controller action**方法返回类型是**object result**(而不是view result)并有异常抛出时.
* 当一个请求为AJAX(Http请求头中`X-Requested-With`为`XMLHttpRequest`)时.
* 当客户端接受的返回类型为`application/json`(Http请求头中`accept` 为`application/json`)时.

如果异常被处理过,则会自动**记录日志**并将格式化的**JSON消息**返回给客户端.

#### 错误消息格式

每个错误消息都是`RemoteServiceErrorResponse` 类的实例.最简单的错误JSON只有一个 **Message** 属性,如下所示:

````json
{
  "error": {
    "message": "This topic is locked and can not add a new message"
  }
}
````

其它**可选字段**可以根据已发生的异常来填充.

##### 错误代码

错误 **代码(code)** 是异常信息中一个有唯一值并可选的字符串值.抛出的异常应实现`IHasErrorCode` 接口来填充该字段.示例JSON如下:

````json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message"
  }
}
````

错误代码同样可用于异常信息的本地化及自定义HTTP状态代码(请参阅下面的相关部分).

##### 错误详细信息

错误的 **详细信息(Details)** 是可选属性.抛出的异常应实现`IHasErrorDetails` 接口来填充该字段.示例JSON如下:

```json
{
  "error": {
    "code": "App:010042",
    "message": "This topic is locked and can not add a new message",
    "details": "A more detailed info about the error..."
  }
}
```

##### 验证错误

当抛出的异常实现`IHasValidationErrors` 接口时,**validationErrors**是一个可被填充的标准字段.示例JSON如下:

````json
{
  "error": {
    "code": "App:010046",
    "message": "Your request is not valid, please correct and try again!",
    "validationErrors": [{
      "message": "Username should be minimum length of 3.",
      "members": ["userName"]
    },
    {
      "message": "Password is required",
      "members": ["password"]
    }]
  }
}
````

`AbpValidationException`已经实现了`IHasValidationErrors`接口,当请求输入无效时,框架会自动抛出此错误. 因此,除非你有自定义的验证逻辑,否则不需要处理验证错误.

#### 日志

被捕获的异常会被自动记录到日志中.

##### 日志级别

默认情况下,记录异常级别为`Error` .可以通过实现`IHasLogLevel` 接口来指定日志的级别,例如:

````C#
public class MyException : Exception, IHasLogLevel
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    //...
}
````

##### 异常自定义日志 

某些异常类型可能需要记录额外日志信息.可以通过实现`IExceptionWithSelfLogging` 接口来记录指定日志,例如:

````C#
public class MyException : Exception, IExceptionWithSelfLogging
{
    public void Log(ILogger logger)
    {
        //...log additional info
    }
}
````

> 扩展方法`ILogger.LogException` 用来记录异常日志. 在需要时可以使用相同的扩展方法.

### 业务异常

大多数异常都是业务异常.可以通过使用`IBusinessException` 接口来标记异常为业务异常.

`BusinessException` 除了实现`IHasErrorCode`,`IHasErrorDetails` ,`IHasLogLevel` 接口外,还实现了`IBusinessException` 接口.其默认日志级别为`Warning`.

通常你会将一个错误代码关联至特定的业务异常.例如:

````C#
throw new BusinessException(QaErrorCodes.CanNotVoteYourOwnAnswer);
````

`QaErrorCodes.CanNotVoteYourOwnAnswer` 是一个字符串常量. 建议使用下面的错误代码格式:

````
<code-namespace>:<error-code>
````

**code-namespace**,应在指定的模块/应用层中保证其唯一.例如:

````
Volo.Qa:010002
````

`Volo.Qa`在这是作为`code-namespace`. `code-namespace` 同样可以在 **本地化** 异常信息时使用.

* 你可以直接抛出一个 `BusinessException` 异常,或者需要时可以从该类派生你自己的Exception类型.
* 对于`BusinessException` 类型,其所有属性都是可选的.但是通常会设置`ErrorCode`或`Message`属性.

### 异常本地化

这里有个问题,就是如何在发送错误消息到客户端时,对错误消息进行本地化.ABP提供了2个模型.

#### 用户友好异常

如果异常实现了 `IUserFriendlyException` 接口,那么ABP不会修改 `Message`和`Details`属性,而直接将它发送给客户端.

`UserFriendlyException` 类是内建的 `IUserFriendlyException` 接口的实现,示例如下:

````C#
throw new UserFriendlyException(
    "Username should be unique!"
);
````

采用这种方式是不需要本地化的.如果需要本地化消息,则可以注入**string localizer**( 请参阅[本地化文档](Localization.md) )来实现. 例:

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage"]);
````

再在本地化资源中为每种语言添加对应的定义.例如:

````json
{
  "culture": "en",
  "texts": {
    "UserNameShouldBeUniqueMessage": "Username should be unique!"
  }
}
````

**string localizer** 支持参数化信息.例如

````C#
throw new UserFriendlyException(_stringLocalizer["UserNameShouldBeUniqueMessage", "john"]);
````

其本地化文本如下:

````json
"UserNameShouldBeUniqueMessage": "Username should be unique! '{0}' is already taken!"
````

* `IUserFriendlyException`接口派生自`IBusinessException`,而 `UserFriendlyException `类派生自`BusinessException`类.

#### 使用错误代码

`UserFriendlyException`很好用,但是在一些高级用法里面,它存在以下问题:

* 在抛出异常的地方必须注入**string localizer** 来实现本地化 .
* 但是,在某些情况下,**可能注入不了string localizer**(比如,在静态上下文或实体方法中)

那么这时就可以通过使用 **错误代码** 的方式来处理本地化,而不是在抛出异常的时候.

首先,在模块配置代码中将 **code-namespace**  映射至 **本地化资源**:

````C#
services.Configure<AbpExceptionLocalizationOptions>(options =>
{
    options.MapCodeNamespace("Volo.Qa", typeof(QaResource));
});
````

然后`Volo.Qa`命名空间下的所有异常都将被对应的本地化资源进行本地化处理. 本地化资源中应包含对应错误代码的文本. 例如:

````json
{
  "culture": "en",
  "texts": {
    "Volo.Qa:010002": "You can not vote your own answer!"
  }
}
````

最后就可以抛出一个包含错误代码的业务异常了:

````C#
throw new BusinessException(QaDomainErrorCodes.CanNotVoteYourOwnAnswer);
````

* 抛出所有实现`IHasErrorCode` 接口的异常都具有相同的行为.因此,对错误代码的本地化,并不是`BusinessException`类所特有的.
* 为错误消息定义本地化文本并不是必须的. 如果未定义,ABP会将默认的错误消息发送给客户端. 而不使用异常的`Message`属性. 如果你想要发送异常的`Message`,使用`UserFriendlyException`(或使用实现`IUserFriendlyException`接口的异常类型)

##### 使用消息的格式化参数

如果有参数化的错误消息,则可以使用异常的`Data`属性进行设置.例如:

````C#
throw new BusinessException("App:010046")
{
    Data =
    {
        {"UserName", "john"}
    }
};

````

另外有一种更为快捷的方式:

````C#
throw new BusinessException("App:010046")
    .WithData("UserName", "john");
````

下面就是一个包含`UserName` 参数的错误消息:

````json
{
  "culture": "en",
  "texts": {
    "App:010046": "Username should be unique. '{UserName}' is already taken!"
  }
}
````

* `WithData` 支持有多个参数的链式调用 (如`.WithData(...).WithData(...)`).

### HTTP状态代码映射

ABP尝试按照以下规则,自动映射常见的异常类型的HTTP状态代码:

* 对于 `AbpAuthorizationException`:
  * 用户没有登录,返回 `401` (未认证).
  * 用户已登录,但是当前访问未授权,返回 `403` (未授权).
* 对于 `AbpValidationException` 返回 `400` (错误的请求) .
* 对于  `EntityNotFoundException`返回 `404` (未找到).
* 对于 `IBusinessException` 和  `IUserFriendlyException` (它是`IBusinessException`的扩展) 返回`403` (未授权) .
* 对于 `NotImplementedException` 返回 `501` (未实现) .
* 对于其他异常 (基础架构中未定义的) 返回 `500` (服务器内部错误) .

`IHttpExceptionStatusCodeFinder` 是用来自动判断HTTP状态代码.默认的实现是`DefaultHttpExceptionStatusCodeFinder`.可以根据需要对其进行更换或扩展.

#### 自定义映射

可以重写HTTP状态代码的自动映射,示例如下:

````C#
services.Configure<AbpExceptionHttpStatusCodeOptions>(options =>
{
    options.Map("Volo.Qa:010002", HttpStatusCode.Conflict);
});
````

### 内置的异常

框架会自动抛出以下异常类型:

- 当用户没有权限执行操作时,会抛出 `AbpAuthorizationException` 异常. 有关更多信息,请参阅授权文档(TODO:link).
- 如果当前请求的输入无效,则抛出`AbpValidationException 异常`. 有关更多信息,请参阅授权文档(TODO:link).
- 如果请求的实体不存在,则抛出`EntityNotFoundException` 异常. 此异常大多数由 [repositories](Repositories.md) 抛出.

你同样可以在代码中抛出这些类型的异常(虽然很少需要这样做)
