using System;
using System.Collections.Generic;
using DL.HelperInterfaces;

namespace DL.DtosV1.Articles
{
    public class EditArticleDto : PostArticleDto, IDeletedMedia
    {
        public IList<Guid> DeletedMediaIds { get; set; }
    }
}