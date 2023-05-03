# Authority Delegation In ABP Commereical

In this post I'll explain new feature from ABP Commercial 7.2.0: Authority Delegation.

## Authority Delegation

Authority Delegation is a way of delegating the responsibility of the current user to a different user(s) for a limited time. Thus, a user can be switched to the delegated users' account and perform actions on their behalf.

> This feature is part of [Account Pro module](https://commercial.abp.io/modules/Volo.Account.Pro).

### Delegate new user

After logging into the application, you can see the `Authority Delegation` menu item under the user menu, clicking the menu will open a modal, in the first tab we can see the delegated users.

![delegated-users](images/delegated-users.jpg)

You can click `Delegate New User` button to delegate a new user:

![delegate-new-user](images/delegate-new-user.jpg)

* You can specify the delegate time range, the delegate is only available within the time range.
* You can multiple delegates to the same user and set different delegate time ranges.
* The delegate has three states: `Expired` `Active` and `Future`.

### My delegated users

You can see a list of users who allowed me to login as them.

![my-delegated-users](images/my-delegated-users.jpg)

You can click the `Login` button to login to the application as a delegated user and go back to my account by clicking on the `Back to my account` icon.

![delegated-impersonate](images/delegated-impersonate.jpg)

> Delegate login uses [impersonation system](https://docs.abp.io/en/commercial/latest/modules/account/impersonation) internally.