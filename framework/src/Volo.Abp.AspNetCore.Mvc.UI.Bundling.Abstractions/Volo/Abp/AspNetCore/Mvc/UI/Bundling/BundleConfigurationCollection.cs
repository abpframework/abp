using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling;

public class BundleConfigurationCollection
{
    private readonly ConcurrentDictionary<string, BundleConfiguration> _bundles;
    private readonly ConcurrentDictionary<string, List<Action<BundleConfiguration>>> _lazyBundleConfigurationActions;
    private readonly List<Action<BundleConfiguration>> _lazyAllBundleConfigurationActions;

    public BundleConfigurationCollection()
    {
        _bundles = new ConcurrentDictionary<string, BundleConfiguration>();
        _lazyBundleConfigurationActions = new ConcurrentDictionary<string, List<Action<BundleConfiguration>>>();
        _lazyAllBundleConfigurationActions = new List<Action<BundleConfiguration>>();
    }

    /// <summary>
    /// Adds a new bundle with given <paramref name="bundleName"/>.
    /// Throws <see cref="AbpException"/> if there is already a bundle with the same name.
    /// </summary>
    /// <param name="bundleName">Bundle name.</param>
    /// <param name="configureAction">Initial configuration action.</param>
    /// <returns>Returns this object for chained calls.</returns>
    public BundleConfigurationCollection Add(
        [NotNull] string bundleName,
        [CanBeNull] Action<BundleConfiguration> configureAction = null)
    {
        if (!TryAdd(bundleName, configureAction))
        {
            throw new AbpException($"There is already a bundle added with given {nameof(bundleName)}: {bundleName}");
        }

        return this;
    }

    /// <summary>
    /// Tries to add a new bundle with given <paramref name="bundleName"/>.
    /// Returns false if it's already created before.
    /// <paramref name="configureAction"/> is not called if it's already added before.
    /// </summary>
    /// <param name="bundleName">Bundle name.</param>
    /// <param name="configureAction">Initial configuration action.</param>
    /// <param name="overrideIfExists">
    /// Overrides the bundle even if it does exists.
    /// This option is designed to be used only for development time
    /// where bundle configuration action may change.
    /// </param>
    /// <returns>Returns true if added. Returns false if it's already added before.</returns>
    public bool TryAdd(
        [NotNull] string bundleName,
        [CanBeNull] Action<BundleConfiguration> configureAction = null,
        bool overrideIfExists = false)
    {
        Check.NotNull(bundleName, nameof(bundleName));

        if (overrideIfExists)
        {
            _bundles[bundleName] = CreateBundle(bundleName, configureAction);
            return true;
        }

        if (_bundles.ContainsKey(bundleName))
        {
            return false;
        }

        var bundle = CreateBundle(bundleName, configureAction);

        if (!_bundles.TryAdd(bundleName, bundle))
        {
            return false;
        }

        return true;
    }

    private BundleConfiguration CreateBundle(string bundleName, Action<BundleConfiguration> configureAction)
    {
        var bundle = new BundleConfiguration(bundleName);

        configureAction?.Invoke(bundle);

        if (_lazyBundleConfigurationActions.TryGetValue(bundleName, out var configurationActions))
        {
            lock (configurationActions)
            {
                configurationActions.ForEach(c => c.Invoke(bundle));
            }
        }

        lock (_lazyAllBundleConfigurationActions)
        {
            _lazyAllBundleConfigurationActions.ForEach(c => c.Invoke(bundle));
        }


        return bundle;
    }

    /// <summary>
    /// Configures an existing bundle.
    /// This method also works for lazy bundles (those are created using razor tag helpers).
    /// </summary>
    /// <param name="bundleName">Bundle name</param>
    /// <param name="configureAction">Configure action</param>
    /// <returns>Returns this object for chained calls.</returns>
    public BundleConfigurationCollection Configure([NotNull] string bundleName, [NotNull] Action<BundleConfiguration> configureAction)
    {
        Check.NotNull(bundleName, nameof(bundleName));
        Check.NotNull(configureAction, nameof(configureAction));

        if (_bundles.TryGetValue(bundleName, out var bundle))
        {
            configureAction.Invoke(bundle);
        }
        else
        {
            var configurationActions = _lazyBundleConfigurationActions
                .GetOrAdd(bundleName, () => new List<Action<BundleConfiguration>>());

            lock (configurationActions)
            {
                configurationActions.Add(configureAction);
            }
        }

        return this;
    }

    /// <summary>
    /// Configures all bundles.
    /// This method also works for lazy bundles (those are created using razor tag helpers).
    /// </summary>
    /// <param name="configureAction">Configure action</param>
    /// <returns>Returns this object for chained calls.</returns>
    public BundleConfigurationCollection ConfigureAll([NotNull] Action<BundleConfiguration> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        foreach (var bundle in _bundles)
        {
            configureAction.Invoke(bundle.Value);
        }

        lock (_lazyAllBundleConfigurationActions)
        {
            _lazyAllBundleConfigurationActions.Add(configureAction);
        }

        return this;
    }

    /// <summary>
    /// Gets a bundle.
    /// </summary>
    /// <param name="bundleName">The bundle name</param>
    /// <returns>The bundle configuration</returns>
    public BundleConfiguration Get([NotNull] string bundleName)
    {
        Check.NotNull(bundleName, nameof(bundleName));

        if (!_bundles.TryGetValue(bundleName, out var bundle))
        {
            throw new AbpException($"There is no bundle with given {nameof(bundleName)}: {bundleName}");
        }

        return bundle;
    }
}
