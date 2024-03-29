﻿using System;
using System.Collections.Generic;
using DL.DtosV1.Blogs;
using DL.DtosV1.Common;
using DL.DtosV1.Users;
using DL.EntitiesV1.Media;
using Newtonsoft.Json;
using NLog.Fluent;

namespace DL.DtosV1.Articles
{
    public class ArticleListDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public OwnerDto Owner { get; set; }
        public LocalizedObject<string> Description { get; set; }
        public DateTime Created { get; set; }
        [JsonIgnore]
        public string DescriptionMapper { get; set; }
        public IDictionary<string, int> Totals { get; set; }
        public BlogTagDto Tag { get; set; }
        public IEnumerable<MediaFile> Media { get; set; }
    }
}