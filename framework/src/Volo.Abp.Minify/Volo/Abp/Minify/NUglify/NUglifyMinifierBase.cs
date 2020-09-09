using System;
using System.Collections.Generic;
using System.Linq;
using NUglify;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Minify.NUglify
{
    public abstract class NUglifyMinifierBase : IMinifier, ITransientDependency
    {
        private static void CheckErrors(UglifyResult result, string originalFileName)
        {
            if (result.HasErrors)
            {
                var errorMessage = "There are some errors on uglifying the given source code!";

                if (originalFileName != null)
                {
                    errorMessage += " Original file: " + originalFileName;
                }

                throw new NUglifyException(
                    $"{errorMessage}{Environment.NewLine}{result.Errors.Select(err => err.ToString()).JoinAsString(Environment.NewLine)}",
                    result.Errors
                );
            }
        }

        public string Minify(
            string source,
            string fileName = null,
            string originalFileName = null)
        {
            var result = UglifySource(source, fileName);
            CheckErrors(result, originalFileName);
            return result.Code;
        }

        protected abstract UglifyResult UglifySource(string source, string fileName);
    }
}