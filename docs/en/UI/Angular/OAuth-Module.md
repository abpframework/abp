# ABP OAuth Package
The authentication functionality has been moved from @abp/ng.core to @abp/ng.ouath since v7.0.

If your app is version 7.0 or higher, you should include "AbpOAuthModule.forRoot()" in your app.module.ts as an import after "CoreModule.forRoot(...)".

Those abstractions can be found in the @abp/ng-core packages.
- `AuthService` (the class that implements the IAuthService interface).
- `NAVIGATE_TO_MANAGE_PROFILE` Inject token.
- `AuthGuard` (the class that implements the IAuthGuard interface).
- `ApiInterceptor` (the class that implements the IApiInterceptor interface).

Those base classes are overridden by the "AbpOAuthModule" for oAuth. Also there are three function that provided with AbpOAuthModule.

- `PIPE_TO_LOGIN_FN_KEY`  that provide to a function that will be called when the user is not authenticated. the function should be  PipeToLoginFn type.
- `SET_TOKEN_RESPONSE_TO_STORAGE_FN_KEY` that provide to a function that will be called when the user is authenticated. the function should be  SetTokenResponseToStorageFn type.
- `CHECK_AUTHENTICATION_STATE_FN_KEY` that provide to a function that will be called when the user is authenticated and store auth state. the function should be CheckAuthenticationStateFn type.
the tokens and interfaces in the `@abp/ng.core` package but the implementation of these interfaces is in the `@abp/ng.oauth` package.

If you want to make your own authentication system, you must also change these 'abstract' classes.

ApiInterceptor is provided by `@abp/ng.core` but overrided with `@abp/ng.oauth`. The ApiInterceptor adds the token, accepted-language, and tenant id to the header of the HTTP request. Also it call http-wait service.
