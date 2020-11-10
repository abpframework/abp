# Angular UI 3.3 to 4.0 Migration Guide

### Removed the Angular Account Module Public UI

Angular UI is using the Authorization Code Flow to authenticate since the version 3.1.0 by default. Starting from the version 4.0, this is becoming the only option, because it is the recommended way of authenticating SPAs.

If you haven't done it yet, see [this post](https://blog.abp.io/abp/ABP-Framework-v3.1-RC-Has-Been-Released) to change the authentication of your application.

### Removed the SessionState

Use `SessionStateService` instead of the `SessionState`. See [this issue](https://github.com/abpframework/abp/issues/5606) for details.