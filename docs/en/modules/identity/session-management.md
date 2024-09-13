# Session Management

The Session Management feature allows you to prevent concurrent login and manage user sessions.

## Prevent concurrent login

There is a setting in the identity section to prevent concurrent login. It has three options:

1. `Disabled`

    No restriction on concurrent login. This is the default.

2. `LogoutFromSameTypeDevices`

    Only one session of the same type can exist. 
    `Same type` means we can restrict single login with a browser, but we may still can login with a mobile application without affecting the browser session. So, for each device type, we may allow a single login.

3. `LogoutFromAllDevices`

    All other sessions will be logged out when a new session is created.

![prevent-concurrent-login](../../images/prevent-concurrent-login.png)

## Manage user sessions

You can view and manage user sessions on the `Users` page of the Identity module.

![user-sessions](../../images/user-sessions.png)
![sessions-modal.png](../../images/sessions-modal.png)
![session-details-modal.png](../../images/session-details-modal.png)

Once you revoke a session, the user will be logged out.

## IdentitySessionCleanupBackgroundWorker

The `IdentitySessionCleanupBackgroundWorker` is a background worker that will remove the sessions that have not been active in the past.

###  IdentitySessionCleanupOptions

* `IsCleanupEnabled`: Default value is `true`. 
* `CleanupPeriod`: Default value is 1 hour.
* `InactiveTimeSpan`: Default value is `30` days.

## How it works

This feature depends on the [Dynamic Claims](https://docs.abp.io/en/abp/latest/Dynamic-Claims) feature of the ABP framework. Here is how it works:

* The `IdentitySessionClaimsPrincipalContributor` will generate a random GUID as a `sessionid` to add the `ClaimsPrincipal`, This usually happens when logging in to get the user's claims.
* The `OnSignedIn` event of `Identity` and `ProcessSignIn` event of `OpenIddict` will get this `sessionid` and store it in the database (`IdentitySession` table).
* The `Dynamic Claims` system's `IdentitySessionDynamicClaimsPrincipalContributor` will ensure the `sessionid` exists or signs out.
* The `IdentitySessionChecker` will check the `sessionid` that exists and update the `LastAccessed` and `IpAddress`  to the cache.
* The `IdentitySessionManager` is used to get one or a list of sessions and update the` LastAccessed` and `IpAddress` from the cache to the database.
* The module will remove the session when logging out.
* The `IdentitySessionCleanupBackgroundWorker` will remove the inactive sessions.
* Once a new session has been created, we will remove the other sessions based on the `PreventConcurrentLogin` setting.
* The `IdentitySessionManager` is used to manage/maintain the sessions. Please use this class instead of directly using the repository.
* The `UpdateSessionAfterCacheHit(10 times by default)` property of `IdentitySessionCheckerOptions` is used to configure the `IdentitySessionChecker` that will update session in the database when cache hits reach this value. This is to reduce the database update frequency and improve performance.
