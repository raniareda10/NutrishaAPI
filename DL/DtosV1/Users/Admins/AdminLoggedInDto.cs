﻿using System.Collections.Generic;
using DL.Repositories.Users.Models;

namespace DL.DtosV1.Users.Admins
{
    public class AdminLoggedInDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string PersonalImage { get; set; }
        public bool IsOwned { get; set; }
        public IEnumerable<string> Permissions { get; set; }

        public static AdminLoggedInDto FromAdminModel(AdminUserModel adminUserModel)
        {
            return new AdminLoggedInDto()
            {
                Email = adminUserModel.Email,
                Id = adminUserModel.Id,
                Name = adminUserModel.Name,
                Language = adminUserModel.Language,
                PersonalImage = adminUserModel.PersonalImage,
                IsOwned = adminUserModel.IsOwned,
                Permissions = adminUserModel.Permissions
            };
        }
    }
}