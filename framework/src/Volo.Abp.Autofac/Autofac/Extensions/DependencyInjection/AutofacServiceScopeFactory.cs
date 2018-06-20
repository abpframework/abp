// This software is part of the Autofac IoC container
// Copyright © 2015 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Autofac.Extensions.DependencyInjection
{
    /// <summary>
    /// Autofac implementation of the ASP.NET Core <see cref="IServiceScopeFactory"/>.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.DependencyInjection.IServiceScopeFactory" />
    [SuppressMessage("Microsoft.ApiDesignGuidelines", "CA2213", Justification = "The creator of the root service lifetime scope is responsible for disposal.")]
    internal class AutofacServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceScopeFactory"/> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacServiceScopeFactory(ILifetimeScope lifetimeScope)
        {
            this._lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Creates an <see cref="IServiceScope" /> which contains an
        /// <see cref="System.IServiceProvider" /> used to resolve dependencies within
        /// the scope.
        /// </summary>
        /// <returns>
        /// An <see cref="IServiceScope" /> controlling the lifetime of the scope. Once
        /// this is disposed, any scoped services that have been resolved
        /// from the <see cref="IServiceScope.ServiceProvider" />
        /// will also be disposed.
        /// </returns>
        public IServiceScope CreateScope()
        {
            return new AutofacServiceScope(this._lifetimeScope.BeginLifetimeScope());
        }
    }
}