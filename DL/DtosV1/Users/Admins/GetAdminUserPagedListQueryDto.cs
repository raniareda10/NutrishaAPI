﻿using DL.CommonModels;

namespace DL.DtosV1.Users.Admins
{
    public class GetAdminUserPagedListQueryDto : GetPagedListQueryModel
    {
        public long? RoleId { get; set; }   
    }
}