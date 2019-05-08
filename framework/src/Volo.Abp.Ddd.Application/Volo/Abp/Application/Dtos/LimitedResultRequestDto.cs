using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        public static int DefaultMaxResultCount { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
    }
}