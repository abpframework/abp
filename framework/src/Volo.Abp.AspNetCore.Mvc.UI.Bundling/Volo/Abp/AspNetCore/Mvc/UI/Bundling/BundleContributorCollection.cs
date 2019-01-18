using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class BundleContributorCollection
    {
        private readonly List<BundleContributor> _contributors;

        public BundleContributorCollection()
        {
            _contributors = new List<BundleContributor>();
        }

        public void Add(BundleContributor contributor)
        {
            foreach (var dependedType in GetDirectDependencies(contributor.GetType()))
            {
                AddWithDependencies(dependedType);
            }

            _contributors.Add(contributor);
        }

        public void Add<TContributor>()
            where TContributor : BundleContributor, new()
        {
            Add(typeof(TContributor));
        }

        public void Add([NotNull] Type contributorType)
        {
            Check.NotNull(contributorType, nameof(contributorType));

            AddWithDependencies(contributorType);
        }

        public IReadOnlyList<BundleContributor> GetAll()
        {
            return _contributors.ToImmutableList();
        }

        private void AddWithDependencies(Type contributorType)
        {
            if (IsAlreadyAdded(contributorType))
            {
                return;
            }

            foreach (var dependedType in GetDirectDependencies(contributorType))
            {
                AddWithDependencies(dependedType); //Recursive call
            }

            AddInstanceToContributors(contributorType);
        }

        private IEnumerable<Type> GetDirectDependencies(Type contributorType)
        {
            var dependsOnAttributes = contributorType
                .GetCustomAttributes(true)
                .OfType<IDependedTypesProvider>()
                .ToList();

            return dependsOnAttributes
                .SelectMany(a => a.GetDependedTypes());
        }

        private bool IsAlreadyAdded(Type contributorType)
        {
            return _contributors.Any(c => c.GetType() == contributorType);
        }

        private void AddInstanceToContributors(Type contributorType)
        {
            if (!typeof(BundleContributor).IsAssignableFrom(contributorType))
            {
                throw new AbpException($"Given {nameof(contributorType)} ({contributorType.AssemblyQualifiedName}) should implement the {typeof(BundleContributor).AssemblyQualifiedName} interface!");
            }

            try
            {
                _contributors.Add((BundleContributor)Activator.CreateInstance(contributorType));
            }
            catch (Exception ex)
            {
                throw new AbpException($"Can not instantiate {contributorType.AssemblyQualifiedName}", ex);
            }
        }
    }
}