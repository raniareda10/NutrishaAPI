using System;
using System.Collections.Generic;
using DL.HelperInterfaces;

namespace DL.DtosV1.BlogVideo
{
    public class EditBlogVideo : PostBlogVideoDto, IDeletedMedia
    {
        public long Id { get; set; }
        public HashSet<Guid> DeletedMediaIds { get; set; }
    }
}