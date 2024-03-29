﻿using System;
using System.Collections.Generic;
using DL.HelperInterfaces;

namespace DL.DtosV1.Articles
{
    public class EditArticleDto : PostArticleDto, IDeletedMedia
    {
        public long Id { get; set; }
        public HashSet<Guid> DeletedMediaIds { get; set; }
    }
}