# Abp Window Service


## Download Blob as File 
AbpWindowService is an Angular service designed to provide utility methods related to window operations. The service has a `downloadBlob` function, which is used for downloading blobs as files within the context of a web application.

### Usage

To make use of the `AbpWindowService` in your Angular application, follow the steps below:

### Injection
Firstly, ensure that the service is injected into the component or any other Angular entity where you wish to use it.

```js
import { AbpWindowService } from '@abp/ng.core';

constructor(private abpWindowService: AbpWindowService) { }
// or
// private abpWindowService   = inject(AbpWindowService)
```

### Downloading a Blob

Once you have the service injected, you can use the downloadBlob method to initiate the download of blob data as a file. For instance:

```js
someMethod() {
  const myBlob = new Blob(["Hello, World!"], { type: "text/plain" });
  this.abpWindowService.downloadBlob(myBlob, "hello.txt");
}
```

### Permissions & Considerations

Ensure that you have appropriate permissions and user interactions before triggering a download. Since downloadBlob initiates a download programmatically, it's best to tie this action to direct user interactions, such as button clicks, to prevent unexpected behaviors or browser restrictions.


### DOCUMENT Token in Service

Angular, being a platform-agnostic framework, is designed to support not only browser-based applications but also other environments like server-side rendering (SSR) through Angular Universal. This design philosophy introduces challenges when accessing global browser-specific objects like window or document directly. To address this, Angular provides a DOCUMENT token that can be used to inject the document object into Angular entities like components and services.

