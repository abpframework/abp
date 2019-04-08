## Abp Nightly Builds

This document will show you how to use the Abp Framework Nightly feed.

There are two URLs, one if you are using NuGet 2 (Visual Studio 2012+) or another URL for NuGet 3 (Visual Studio 2015+).

- NuGet v3 Feed URL: <https://www.myget.org/F/abp-nightly/api/v3/index.json>
- NuGet v2 Feed URL: <https://www.myget.org/F/abp-nightly/api/v2/>

Go to: `Tools > Options > NuGet Package Manager > Package Source`

Once you reached that window:

1. Click the green `+` icon
2. In the bottom, name the feed (instead of `Package source`) and paste in the URL (instead of `http://packagesource`)
3. Click the `Update` button
4. Click `OK`

Then open the **Package Manager Console** and from `Package source` drop-down menu, select the new package source you added. If you choose the “abp-nightly” feed and check the `Include prerelease` checkbox.
