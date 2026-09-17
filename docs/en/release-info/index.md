```json
//[doc-seo]
{
    "Description": "Understand ABP Platform's versioning, release schedule, LTS support policy, and how we handle breaking changes to ensure smooth upgrades for your applications."
}
```

# Versioning & Releases

ABP Platform follows a predictable release cycle aligned with .NET releases. This document explains our versioning strategy, release schedule, support policy, and how we handle breaking changes.

## Our Commitment

As a framework you build upon, ABP must be both reliable and evolving — stable enough to trust for long-term projects, yet continuously improving with new features and the latest .NET advancements. To achieve this balance, we commit to:

* **Predictable releases**: Major versions annually, aligned with .NET releases
* **Long-term support**: Every major version receives 2 years of support
* **Smooth upgrades**: Comprehensive migration guides and tooling for version updates
* **Transparent communication**: Clear documentation of breaking changes and deprecations

## ABP Versioning

ABP version numbers indicate the level of changes that are introduced by the release. This use of [semantic versioning](https://semver.org/) helps you understand the potential impact of updating to a new version.

ABP version numbers have three parts: `major.minor.patch`. For example, version 10.1.2 indicates major version 10, minor version 1, and patch level 2.

The version number is incremented based on the level of change included in the release.

| Level of change | Details |
| --- | --- |
| **Major release** | Contains significant new features. Aligned with the new major .NET release. Some developer assistance is expected. You should check the [migration guide](migration-guides/index.md) and possibly refactor code to adapt to new APIs. |
| **Minor release** | Contains new features and improvements. Minor releases are generally backward-compatible; minimal developer assistance is expected, but you can optionally modify your applications to begin using new APIs and features. |
| **Patch release** | Low risk, bug fix and security patch release. No developer assistance is expected. |

> **Note:** ABP version is aligned with .NET version. For example, ABP 10.x runs on .NET 10, ABP 9.x runs on .NET 9.

### Preview Releases

We provide preview releases for each major and minor release so you can try new features before the stable release:

| Pre-release type | Details |
| --- | --- |
| **Release Candidate (RC)** | A release that is feature complete and in final testing. RC releases are indicated by a release tag appended with the `-rc` identifier, such as `10.1.0-rc.1`. |
| **Nightly builds** | The latest development builds published every weekday night. Nightly builds allow you to try the previous day's development. |

See the [Preview Releases](previews.md) and [Nightly Builds](nightly-builds.md) documents for more information.

## Release Frequency

We work toward a regular schedule of releases, so that you can plan and coordinate your updates with the continuing evolution of ABP and the .NET platform.

> **Note:** Dates are offered as general guidance and are subject to change.

In general, expect the following release cycle:

* **A major release once a year**, typically in November, following the new major .NET release
* **2-4 minor releases** for each major version, released every ~3 months after the major release
* **Patch releases** as needed, typically every 2-4 weeks for the latest minor version

This cadence of releases gives eager developers access to new features as soon as they are fully developed and tested, while maintaining the stability and reliability of the platform for production users.

### Release Schedule

| Version | Status | Released | Active Ends | LTS Ends |
| --- | --- | --- | --- | --- |
| ^10.0.0 | Active | 2025-11 | 2026-11 | 2027-11 |
| ^9.0.0 | LTS | 2024-11 | 2025-11 | 2026-11 |


See the [Release Notes](release-notes.md) for detailed information about each release.

## Support Policy and Schedule (LTS)

ABP Platform follows a **Long-Term Support (LTS)** policy to ensure your applications remain secure and stable over time.

### Support Window

Every major version has a **2-year lifecycle** with two distinct phases:

| Support Stage | Duration | Details |
| --- | --- | --- |
| **Active** | ~1 year | The version is under active development. Regularly-scheduled updates and patches are released. New features and improvements are added in minor versions. |
| **Long-Term Support (LTS)** | ~1 year | Only critical fixes and security patches are released. No new features are added. |

This means we actively develop a major version for about 1 year (until the next major .NET release), then provide LTS support for another year.

### LTS Fixes

As a general rule, a fix is considered for an LTS version if it resolves one of:

* A newly identified security vulnerability
* A critical bug that significantly impacts production applications
* A regression caused by a 3rd party change, such as a new browser version or dependency update

## Deprecation Policy

When the ABP team intends to remove an API or feature, it will be marked as *deprecated*. This occurs when an API is obsolete, superseded by another API, or otherwise discontinued. Deprecated APIs remain available through their deprecated phase, which lasts a minimum of one major version (approximately one year).

To help ensure that you have sufficient time and a clear path to update, this is our deprecation policy:

| Deprecation Stage | Details |
| --- | --- |
| **Announcement** | We announce deprecated APIs and features in the [release notes](release-notes.md) and [migration guides](migration-guides/index.md). Deprecated APIs are typically marked with `[Obsolete]` attribute in the code, which enables IDEs to provide warnings if your project depends on them. We also announce a recommended update path. |
| **Deprecation period** | When an API or feature is deprecated, it is still present in at least the next major release. After that, deprecated APIs and features are candidates for removal. A deprecation can be announced in any release, but the removal of a deprecated API or feature happens only in major releases. |
| **NuGet/NPM dependencies** | We typically make dependency updates that require changes to your applications in major releases. In minor releases, we may update dependencies by expanding the supported versions, but we try not to require projects to update these dependencies until the next major version. |

## Breaking Changes Policy

Breaking changes require you to do work because the state after the change is not backward compatible with the state before it. Examples of breaking changes include the removal of public APIs, changes to method signatures, changing the timing of events, or updating to a new version of a dependency that includes breaking changes itself.

### How We Handle Breaking Changes

To support you in case of breaking changes:

* We follow our [deprecation policy](#deprecation-policy) before we remove a public API
* We provide detailed [migration guides](migration-guides/index.md) when a version includes breaking changes

### Update Path

We recommend updating one major version at a time for a smoother upgrade experience. For example, to update from version 8.x to version 10.x:

1. Update from version 8.x to version 9.x
2. Update from version 9.x to version 10.x

See the [upgrading](upgrading.md) document for detailed instructions on how to upgrade your solutions.

## Related Documents

* [Release Notes](release-notes.md) - Detailed release notes for each version
* [Migration Guides](migration-guides/index.md) - Step-by-step guides for upgrading between versions
* [Road Map](road-map.md) - Upcoming features and planned releases
* [Upgrading](upgrading.md) - How to upgrade your ABP-based solutions
* [Preview Releases](previews.md) - Information about preview/RC releases
* [Nightly Builds](nightly-builds.md) - How to use nightly builds
* [Official Packages](https://abp.io/packages) - Browse all ABP packages
