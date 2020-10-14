﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Volo.Abp
{
    [DebuggerStepThrough]
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrWhiteSpace())
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }
        [ContractAnnotation("value:null => null")]
        public static string IsDigit(
            string value,
            [InvokerParameterName][NotNull] string parameterName)
        {
            if (parameterName.IsNullOrEmpty())
            {
                throw new ArgumentException($"parameterName can not be null or empty!");
            }
            if (!value.IsNullOrEmpty() && !Regex.IsMatch(value, @"^\d+$"))
            {
                throw new ArgumentException($"each character of {parameterName} must be in the range 0 through 9!", parameterName);
            }
            return value;
        }
        
    }
}
