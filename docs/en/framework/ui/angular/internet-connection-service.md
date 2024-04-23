# Internet Connection Service
`InternetConnectionService` is a service which is exposed by the `@abp/ng.core` package. **You can use this service in order to check your internet connection**

## Getting Started
When you inject the InternetConnectionService you can get the current internet status, and it gets immediately updated if the status changes.

`InternetConnectionService` provides two choices to catch the network status:
1. Signal (readonly)
2. Observable


# How To Use
İt's easy, just inject the service and get the network status.

**You can get via signal**
```ts
class SomeComponent{
 internetConnectionService = inject(InternetConnectionService);
 isOnline = this.internetConnectionService.networkStatus
}
```
**or you can get the observable**
```ts
class SomeComponent{
 internetConnectionService = inject(InternetConnectionService);
 isOnline = this.internetConnectionService.networkStatus$
}
```

To see how we implement to the template, check the `InternetConnectionStatusComponent`
