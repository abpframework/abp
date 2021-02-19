using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.IdentityServer
{
    /// <summary>
    /// Default implementation of redirect URI validator. Validates the URIs against
    /// the client's configured URIs.
    /// </summary>
    public class AbpStrictRedirectUriValidator : IRedirectUriValidator
    {
        protected AbpRedirectUriValidatorOptions Options { get; }

        public AbpStrictRedirectUriValidator(IOptions<AbpRedirectUriValidatorOptions> options)
        {
            Options = options.Value;
        }

        /// <summary>
        /// Checks if a given URI string is in a collection of strings (using ordinal ignore case comparison)
        /// </summary>
        /// <param name="uris">The uris.</param>
        /// <param name="requestedUri">The requested URI.</param>
        /// <returns></returns>
        protected virtual bool StringCollectionContainsString(IEnumerable<string> uris, string requestedUri)
        {
            if (uris == null)
            {
                return false;
            }

            foreach (var url in uris)
            {
                if (url.Contains(requestedUri, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (url.Contains(Options.DomainFormat))
                {
                    var extractResult = FormattedStringValueExtracter.Extract(requestedUri, url, ignoreCase: true);
                    if (extractResult.IsMatch)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether a redirect URI is valid for a client.
        /// </summary>
        /// <param name="requestedUri">The requested URI.</param>
        /// <param name="client">The client.</param>
        /// <returns>
        ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
        /// </returns>
        public virtual Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(StringCollectionContainsString(client.RedirectUris, requestedUri));
        }

        /// <summary>
        /// Determines whether a post logout URI is valid for a client.
        /// </summary>
        /// <param name="requestedUri">The requested URI.</param>
        /// <param name="client">The client.</param>
        /// <returns>
        ///   <c>true</c> is the URI is valid; <c>false</c> otherwise.
        /// </returns>
        public virtual Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            return Task.FromResult(StringCollectionContainsString(client.PostLogoutRedirectUris, requestedUri));
        }
    }
}
