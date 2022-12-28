# ABP OAuth Package
The authentication functionality has been moved from @abp/ng.core to @abp/ng.ouath since v7.0.

If your app is version 7.0 or higher, you should include "AbpOAuthModule.forRoot()" in your app.module.ts as an import after "CoreModule.forRoot(...)".

Those abstractions can be found in the @abp/ng-core packages.
- `AuthService` (the class that implements the IAuthService interface).
- `NAVIGATE_TO_MANAGE_PROFILE` Inject token.
- `AuthGuard` (the class that implements the IAuthGuard interface).

Those base classes are overridden by the "AbpOAuthModule" for oAuth.
If you want to make your own authentication system, you must also change these 'abstract' classes.

ApiInterceptor is provided by @abp/ng.oauth. The ApiInterceptor adds the token, accepted-language, and tenant id to the header of the HTTP request.
