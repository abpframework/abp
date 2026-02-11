# Installation Notes for Identity Server Module

IdentityServer module provides a full integration with the [IdentityServer4](https://github.com/DuendeArchive/IdentityServer4) (IDS) framework, which provides advanced authentication features like single sign-on and API access control. This module persists clients, resources and other IDS-related objects to database. This module is replaced by [OpenIddict](https://abp.io/docs/latest/modules/openiddict) module after ABP v6.0 in the startup templates.

> Note: You can not use IdentityServer and OpenIddict modules together. They are separate OpenID provider libraries for the same job.

## Documentation

For detailed information and usage instructions, please visit the [Identity Server Module documentation](https://abp.io/docs/latest/modules/identity-server). 