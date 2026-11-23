# Migrating from jQuery 3 to jQuery 4

## Introduction

ABP 10.7 upgraded the MVC / Razor Pages UI from jQuery 3.7 to jQuery 4.0 and loaded [jQuery Migrate](https://github.com/jquery/jquery-migrate) right after it, so scripts that used the APIs removed in jQuery 4 kept working. Starting with the release that includes [#26027](https://github.com/abpframework/abp/pull/26027), ABP no longer loads jQuery Migrate. Calls to the removed APIs then fail.

This document explains how to find and update these calls. See the [jQuery 4.0 upgrade guide](https://jquery.com/upgrade-guide/4.0/) for all changes in jQuery 4.

## What Changed

- `@abp/jquery` depends on jQuery 4.0 since ABP 10.7.
- `@abp/jquery` no longer depends on `jquery-migrate`, and `JQueryScriptContributor` no longer adds `/libs/jquery-migrate/jquery-migrate.js` to the bundles.
- `@abp/select2` moves from select2 4.0.13 to 4.1.1. select2 4.0.13 calls the removed `$.isArray` and doesn't work with jQuery 4 without jQuery Migrate.

The APIs that jQuery 4 only deprecated, such as event shorthands like `.click()` and `.bind()`, still work.

## Find the Code to Update

- **Upgrading from ABP 10.7 to 11.0.0:** these versions load jQuery Migrate. Open your pages in Development and check the browser console for `JQMIGRATE: ... is removed` warnings. Each warning is a call that fails after the upgrade.
- **Upgrading from an earlier version:** these versions use jQuery 3, so there is no warning. Search your scripts for the APIs below, and check the browser console after the upgrade for errors such as `$.trim is not a function`.

Check the jQuery plugins that your application adds itself, too. In production, ABP combines the global scripts into one bundle, so an error while a script loads also stops the scripts after it in that bundle. Test the pages in Development before you publish.

## Replace the Removed APIs

| Removed API | Use instead |
| --- | --- |
| `$.trim(value)` | `(value \|\| '').trim()` |
| `$.isArray(value)` | `Array.isArray(value)` |
| `$.isFunction(value)` | `typeof value === 'function'` |
| `$.parseJSON(text)` | `JSON.parse(text)` |
| `$.now()` | `Date.now()` |
| `$.unique(elements)` | `$.uniqueSort(elements)` |

## select2 4.1

select2 4.1 removes and renames some options, for example `containerCssClass` is now `selectionCssClass`, and it changes the selection markup. The Tab key now selects the highlighted option. See the [select2 4.1.0 release notes](https://github.com/select2/select2/releases/tag/4.1.0).

select2 4.1 doesn't read the page direction and renders `dir="undefined"`, so its RTL styles don't apply. `Select2ScriptContributor` adds `/libs/select2/js/abp.select2.js`, which restores the direction lookup. If your pages load select2 without the ABP bundles, load this file right after select2.

## Update the Client Libraries

Run `abp install-libs` in your MVC web projects after updating the ABP npm packages, then rebuild. If you keep `wwwroot/libs` in source control, commit the updated files. The old select2 4.0.13 files don't work without jQuery Migrate.

## Keep jQuery Migrate for a While

If you can't update all scripts before the upgrade, load jQuery Migrate yourself until you do:

1. Add `"jquery-migrate": "~4.0.2"` to the `dependencies` of your web project's `package.json`.
2. Add the files to the `mappings` of `abp.resourcemapping.js`, then run `abp install-libs`:

   ```js
   "@node_modules/jquery-migrate/dist/jquery-migrate.js": "@libs/jquery-migrate/",
   "@node_modules/jquery-migrate/dist/jquery-migrate.min.js": "@libs/jquery-migrate/"
   ```

3. Add a bundle contributor that loads it right after jQuery, and add it to the global bundle in your web module:

   ```csharp
   using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
   using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;

   public class JQueryMigrateScriptContributor : BundleContributor
   {
       public override void ConfigureBundle(BundleConfigurationContext context)
       {
           var index = context.Files.FindIndex(f => f.FileName == "/libs/jquery/jquery.js");
           context.Files.Insert(index + 1, "/libs/jquery-migrate/jquery-migrate.js");
       }
   }
   ```

   ```csharp
   Configure<AbpBundlingOptions>(options =>
   {
       options.ScriptBundles.Configure(StandardBundles.Scripts.Global, bundle =>
       {
           bundle.AddContributors(typeof(JQueryMigrateScriptContributor));
       });
   });
   ```

Remove these steps once your scripts no longer use the removed APIs.
