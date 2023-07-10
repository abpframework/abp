# Internet Connection Service
`InternetConnectionService` is a service which is exposed by `@abp/ng.core` package. **If you want to detect whether you are connected to the internet or not, you can use this service.**


### internet-connection-service.ts
```ts
import { DOCUMENT } from  '@angular/common';
import { Injectable, computed, inject, signal } from  '@angular/core';
import { BehaviorSubject } from  'rxjs';

@Injectable({
  providedIn: 'root',
})
export  class  InternetConnectionService{
 protected readonly window = inject(DOCUMENT).defaultView;
 protected readonly navigator = this.window.navigator;

 /* BehaviorSubject */
 private  status$ = new BehaviorSubject<boolean>(navigator.onLine)

 /* creates writableSignal */
 private  status = signal(navigator.onLine);

 /* READONLY ANGULAR SIGNAL */
 networkStatus = computed(() =>  this.status())

 constructor(){
  this.window.addEventListener('offline', () =>  this.setStatus());
  this.window.addEventListener('online', () =>  this.setStatus());
 }

 private setStatus(){
  this.status.set(navigator.onLine)
  this.status$.next(navigator.onLine)
 }
 
 /* returns OBSERVABLE */
 get networkStatus$(){
  return this.status$.asObservable()
 }
}
```

## Getting Started
When you inject InternetConnectionService you can get the current internet status also if status changes you can catch them immediately.

As you can see from the code above, `InternetConnectionService` is providing 2 alternatives for catching network status;
1. Signal (readonly)
2. Observable


# How To Use
İt's easy just inject the service and get network status

**You can get via signal**
```ts
class someComponent{
 internetConnectionService  =  inject(InternetConnectionService);
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

That's all, feel free to try on your template
