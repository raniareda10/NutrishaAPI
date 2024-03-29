﻿using System.Collections.Generic;
using DL.DtosV1.Storage;

namespace DL.HelperInterfaces
{
    public interface IMedia : IFiles, IExternalMedia
    {
    }

    public interface IFiles
    {
        public IList<FormFileDto> Files { get; set; }
    }

    public interface IExternalMedia
    {
        public IList<ExternalMedia> ExternalMedia { get; set; }
    }
}