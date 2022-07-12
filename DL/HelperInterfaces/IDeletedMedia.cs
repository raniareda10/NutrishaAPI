using System;
using System.Collections.Generic;

namespace DL.HelperInterfaces
{
    public interface IDeletedMedia
    {
        public HashSet<Guid> DeletedMediaIds { get; set; }
    }
}