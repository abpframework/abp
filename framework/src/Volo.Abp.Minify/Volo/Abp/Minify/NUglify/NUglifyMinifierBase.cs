using System;
using System.Collections.Generic;
using System.Linq;
using NUglify;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Minify.NUglify
{
    public abstract class NUglifyMinifierBase : IMinifier, ITransientDependency
    {
        private static void CheckErrors(UglifyResult result)
        {
            if (result.HasErrors)
            {
                throw new NUglifyException(
                    $"There are some errors on uglifying the given source code!{Environment.NewLine}{result.Errors.Select(err => err.ToString()).JoinAsString(Environment.NewLine)}",
                    result.Errors
                );
            }
        }

        public string Minify(string source, string fileName = null)
        {
            var result = UglifySource(source, fileName);
            CheckErrors(result);
            return result.Code;
        }

        protected abstract UglifyResult UglifySource(string source, string fileName);
    }
}