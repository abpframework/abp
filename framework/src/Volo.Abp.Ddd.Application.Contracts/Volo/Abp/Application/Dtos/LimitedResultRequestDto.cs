using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Localization.Resources.AbpDdd;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class LimitedResultRequestDto : ILimitedResultRequest, IValidatableObject
    {
        /// <summary>
        /// Default value: 10.
        /// </summary>
        public static int DefaultMaxResultCount { get; set; } = 10;

        /// <summary>
        /// Maximum possible value of the <see cref="MaxResultCount"/>.
        /// Default value: 1,000.
        /// </summary>
        public static int MaxMaxResultCount { get; set; } = 1000;

        /// <summary>
        /// Maximum result count should be returned.
        /// This is generally used to limit result count on paging.
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxResultCount > MaxMaxResultCount)
            {
                var localizer = validationContext.GetRequiredService<IStringLocalizer<AbpDddApplicationContractsResource>>();

                yield return new ValidationResult(
                    localizer[
                        "MaxResultCountExceededExceptionMessage",
                        nameof(MaxResultCount),
                        MaxMaxResultCount,
                        typeof(LimitedResultRequestDto).FullName,
                        nameof(MaxMaxResultCount)
                    ],
                    new[] { nameof(MaxResultCount) });
            }
        }
    }

    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class ExtensibleLimitedResultRequestDto : ExtensibleEntityDto, ILimitedResultRequest, IValidatableObject
    {
        /// <summary>
        /// Default value: 10.
        /// </summary>
        public static int DefaultMaxResultCount { get; set; } = 10;

        /// <summary>
        /// Maximum possible value of the <see cref="MaxResultCount"/>.
        /// Default value: 1,000.
        /// </summary>
        public static int MaxMaxResultCount { get; set; } = 1000;

        /// <summary>
        /// Maximum result count should be returned.
        /// This is generally used to limit result count on paging.
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach(var result in base.Validate(validationContext))
            {
                yield return result;
            }

            if (MaxResultCount > MaxMaxResultCount)
            {
                var localizer = validationContext.GetRequiredService<IStringLocalizer<AbpDddApplicationContractsResource>>();

                yield return new ValidationResult(
                    localizer[
                        "MaxResultCountExceededExceptionMessage",
                        nameof(MaxResultCount),
                        MaxMaxResultCount,
                        typeof(ExtensibleLimitedResultRequestDto).FullName,
                        nameof(MaxMaxResultCount)
                    ],
                    new[] { nameof(MaxResultCount) });
            }
        }
    }
}
