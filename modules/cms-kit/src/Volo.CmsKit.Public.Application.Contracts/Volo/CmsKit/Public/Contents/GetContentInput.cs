﻿using System;

namespace Volo.CmsKit.Public.Contents
{
    [Serializable]
    public class GetContentInput
    {
        public string EntityType { get; set; }
        
        public string EntityId { get; set; }
    }
}
