# Internet Connection Service
`InternetConnectionService` is a service which is exposed by `@abp/ng.core` package. **If you want to detect whether you are connected to the internet or not, you can use this service.**

## Getting Started
When you inject InternetConnectionService you can get the current internet status also if status changes you can catch them immediately.

`InternetConnectionService` is providing 2 alternatives for catching network status;
1. Signal (readonly)
2. Observable


# How To Use
İt's easy just inject the service and get network status.

**You can get via signal**
```ts
class someComponent{
 internetConnectionService = inject(InternetConnectionService);
 isOnline = this.internetConnectionService.networkStatus
}
```
**or you can get as observable**
```ts
class someComponent{
 internetConnectionService = inject(InternetConnectionService);
 isOnline = this.internetConnectionService.networkStatus$
}
```

To see how we implement to template, check `InternetConnectionStatusComponent`
