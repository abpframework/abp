# Chat Module (Pro)

> You must have an ABP Team or a higher license to use this module.

This module implements real time messaging between users for an application.

See [the module description page](https://abp.io/modules/Volo.Chat) for an overview of the module features.

To download the source-code of the Chat module, you can use ABP Suite or you can use the ABP CLI with the following command:

```bash
abp get-source Volo.Chat
```

## How to Install

Chat module is not installed in [the startup templates](../solution-templates). So, it needs to be installed manually. There are two ways of installing a module into your application.

### Installation

#### 1. Using ABP Suite

ABP Suite allows adding a module to a solution using ``Add to your solution`` option in modules list.
![suite-chat-add](../images/suite-chat-add.png)

#### 2. Manual Installation

If you modified your solution structure, adding module using ABP Suite might not work for you. In such cases, chat module can be added to a solution manually.

In order to do that, add packages listed below to matching project on your solution. For example, ```Volo.Chat.Application``` package to your **{ProjectName}.Application.csproj** like below;

```json
<PackageReference Include="Volo.Chat.Application" Version="x.x.x" />
```

After adding the package reference, open the module class of the project (eg: `{ProjectName}ApplicationModule`) and add the below code to the `DependsOn` attribute.

```csharp
[DependsOn(
  //...
  typeof(ChatApplicationModule)
)]
```

> If you are using Blazor Web App, you need to add the `Volo.Chat.Blazor.WebAssembly` package to the **{ProjectName}.Blazor.Client.csproj** project and ad the `Volo.Chat.Blazor.Server` package to the **{ProjectName}.Blazor.csproj** project.

The `Volo.Chat.SignalR` package must be added according to your project structure:

* **Mvc**
  * **Tiered**: {ProjectName}.HttpApi.Host project.
* **Blazor Server**
  * **Unified Backend**: {ProjectName}.Blazor.
  * **Separated Auth Server**: {ProjectName}.HttpApi.Host project.
* **Blazor WebAssembly**: {ProjectName}.HttpApi.Host project. 
* **Blazor Web App**
  * **Unified Backend**: {ProjectName}.Blazor.
  * **Separated Auth Server**: {ProjectName}.HttpApi.Host project.
* **Angular**: {ProjectName}.HttpApi.Host project.

If database provider of your project is **EntityFrameworkCore**, use `modelBuilder.ConfigureChat()` to configure database tables in your project's DbContext.

## Configuration

### SignalR Access Token Configuration for Angular and Blazor WebAssembly Projects

See [Microsoft SignalR Authentication and Authorization](https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz) and [Microsoft SignalR Security](https://docs.microsoft.com/en-us/aspnet/core/signalr/security#access-token-logging) documentations.

> In standard web APIs, bearer tokens are sent in an HTTP header. However, SignalR is unable to set these headers in browsers when using some transports. When using WebSockets and Server-Sent Events, the token is transmitted as a query string parameter. To support this on the server, additional configuration is required.
>

Here is a sample configuration for this:

```csharp
app.Use(async (httpContext, next) =>
{
    var accessToken = httpContext.Request.Query["access_token"];

    var path = httpContext.Request.Path;
    if (!string.IsNullOrEmpty(accessToken) &&
        (path.StartsWithSegments("/signalr-hubs/chat")))
    {
        httpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
    }

    await next();
});
```

### Adding Distributed Event Bus for distributed architecture projects

When Web & API tiers are separated, it is impossible to directly send a server-to-client message from the HTTP API. This is also true for a microservice architected application. **Chat Module** uses the distributed event bus to deliver the message from API application to the web application, then to the client.

If your project has such an architecture (example: MVC + tiered option), then you need a Distributed Event Bus. See [related ABP documentation](../framework/infrastructure/event-bus/distributed) to understand Distributed Event Bus system in ABP. Also see [RabbitMQ Integration documentation](../framework/infrastructure/event-bus/distributed/rabbitmq.md) if your choice is to use RabbitMQ.

## Packages

This module follows the [module development best practices guide](../framework/architecture/best-practices) and consists of several NuGet and NPM packages. See the guide if you want to understand the packages and relations between them.

You can visit [Chat module package list page](https://abp.io/packages?moduleName=Volo.Chat) to see list of packages related with this module.

## User interface

### Manage chat feature

Chat module defines the chat feature, you need to enable the chat feature to use chat.

![chat-feature](../images/chat-feature.png)

### Chat page

This is the page that users send messages to each other.

![chat-page](../images/chat-page.png)

### Chat icon on navigation bar

An icon that shows unread message count of the user and leads to chat page when clicked is added to navigation menu.

![chat-page](../images/chat-icon.png)

## Internals

### Domain layer

#### Entities and aggregate roots

* `Message` (aggregate root): Represents a chat message. Implements `IMultiTenant`.
  * `Text`: Message content.
  * `IsAllRead`: Message read information.
  * `ReadTime`: Message read time information.
* `ChatUser` (aggregate root): Represents a chat user. Implements `IUser` and `IUpdateUserData` interfaces.
* `UserMessage`: is created for each side (sender and receiver) of a message. Implements `IMultiTenant` and `IAggregateRoot` interfaces.
  *  `ChatMessageId`: Id of related `Message`.
  *  `UserId`: Id of related `ChatUser.`
  *  `TargetUserId`: Id of other related `ChatUser`.
  *  `Side`: States that if it is a send message or received message.
  *  `IsRead`: Read information.
  *  `ReadTime`: Read time information.
* `Conversation`: is created for each side of a conversation between users. Implements `IMultiTenant` and `IAggregateRoot` interfaces.
  *  `UserId`: Id of related `ChatUser.`
  *  `TargetUserId`: Id of other related `ChatUser`.
  *  `LastMessageSide`: States the side of the latest message (send or received).
  *  `LastMessage`: Content of the last message in the conversation.
  *  `LastMessageDate`: Date of the last message in the conversation.
  *  `UnreadMessageCount`: Count of unread messages in the conversation.

#### Repositories

This module follows the [Repository Best Practices & Conventions](../framework/architecture/best-practices) guide.

Following custom repositories are defined for this module:

* `IConversationRepository`
* `IUserMessageRepository`
* `IChatUserRepository`
* `IMessageRepository`

#### Domain Services

* `MessagingManager`

### Application layer

#### Application services

* `ConversationAppService` (implements `IConversationAppService`): Used to send messages, get conversation between users and mark a conversation as read.
* `SettingsAppService` (implements `ISettingsAppService`): Used to save chat settings.
* `ContactAppService` (implements `IContactAppService`): Used to get list of contacts and total unread message count of a user.
* `DistributedEventBusRealTimeChatMessageSender` (implements `IRealTimeChatMessageSender`): Used to publish chat messages to distributed event bus.
* `SignalRRealTimeChatMessageSender` (implements `IRealTimeChatMessageSender`): Used to send messages to target `SignalR` client.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Chat` prefix by default. Set static properties on the `ChatDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `Chat` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](../framework/fundamentals/connection-strings.md) documentation for details.

#### Entity Framework Core

##### Tables

* **ChatUsers**
* **ChatMessages**
* **ChatUserMessages**
* **ChatConversations**

#### MongoDB

##### Collections

* **ChatUsers**
* **ChatMessages**
* **ChatUserMessages**
* **ChatConversations**


### Angular UI

#### Installation

In order to configure the application to use the `ChatModule`, you first need to import `ChatConfigModule` from `@volo/abp.ng.chat/config` to root module. `ChatConfigModule` has a static `forRoot` method which you should call for a proper configuration.

```js
// app.module.ts
import { ChatConfigModule } from '@volo/abp.ng.chat/config';

@NgModule({
  imports: [
    // other imports
    ChatConfigModule.forRoot(),
    // other imports
  ],
  // ...
})
export class AppModule {}
```

The `ChatModule` should be imported and lazy-loaded in your routing module. It has a static `forLazy` method for configuration. It is available for import from `@volo/abp.ng.chat`.

```js
// app-routing.module.ts
const routes: Routes = [
  // other route definitions
  {
    path: 'chat',
    loadChildren: () =>
      import('@volo/abp.ng.chat').then(m => m.ChatModule.forLazy(/* options here */)),
  },
];

@NgModule(/* AppRoutingModule metadata */)
export class AppRoutingModule {}
```

#### Services / Models

Chat module services and models are generated via `generate-proxy` command of the [ABP CLI](../cli). If you need the module's proxies, you can run the following command in the Angular project directory:

```bash
abp generate-proxy --module chat
```

#### Remote Endpoint URL

The Chat module remote endpoint URLs can be configured in the environment files.

```js
export const environment = {
  // other configurations
  apis: {
    default: {
      url: 'default url here',
    },
    Chat: {
      url: 'Chat remote url here',
      signalRUrl: 'Chat signalR remote url here',
    },
    // other api configurations
  },
};
```

The Chat module remote URL configurations shown above are optional.

> If you don't set the `signalRUrl`, `Chat.url` will be used as fallback. If you don't set the `Chat` property, the `default.url` will be used as fallback.

### Blazor WebAssembly UI

#### Remote Endpoint URL


The Chat module remote endpoint URLs can be configured via the `ChatBlazorWebAssemblyOptions`.

```csharp
Configure<ChatBlazorWebAssemblyOptions>(options =>
{
    options.SignalrUrl = builder.Configuration["RemoteServices:Chat:BaseUrl"];
});
```

```json
"RemoteServices": {
  "Default": {
    "BaseUrl": "Default url here"
  },
  "Chat": {
    "BaseUrl": "Chat remote url here"
  }
},
```

> If you don't set the `BaseUrl`, the `Default.BaseUrl` will be used as fallback.

## Distributed Events

This module defines an event for messaging. It is published when a new message is sent from a user to another user, with an Event Transfer Object type of `ChatMessageEto`. See the [standard distributed events](../framework/infrastructure/event-bus/distributed) for more information about distributed events.
