using System;
using Volo.CmsKit.Entities;

namespace Volo.CmsKit.Contents
{
    public static class ContentConsts
    {
        public static int MaxEntityTypeLength { get; set; } = CmsEntityConsts.MaxEntityTypeLength;

        public static int MaxEntityIdLength { get; set; } = CmsEntityConsts.MaxEntityIdLength;
        
        // TODO: consider
        public static int MaxValueLength { get; set; } = int.MaxValue;
    }
}