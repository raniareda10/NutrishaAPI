using System;
using System.Collections.Generic;

namespace DL.HelperInterfaces
{
    public interface IDeletedMedia
    {
        public IList<Guid> DeletedMediaIds { get; set; }
    }
}