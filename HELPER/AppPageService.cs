using AutoMapper;
using BL.Helpers;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;

using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.MailModels;
using HELPER;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using Model.ApiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HELPER
{

    public interface IAppPageService
    {
        public PageSetting getPageSetting(string appsettingpageCode);

    }


    public class AppPageService : IAppPageService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AppPageService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }



        public PageSetting getPageSetting(string appsettingpageCode)
        {
            var appSettingItem = _unitOfWork.ApplicationPageRepository.Get(dt => dt.pageCode == appsettingpageCode);
            if(appSettingItem != null)
            {
                var pageSettngItem = _unitOfWork.PageSettingRepository.Get(dt => dt.ApplicationPageId == appSettingItem.Id);

                if(pageSettngItem != null)
                {
                    return pageSettngItem;
                }
            }






            return null;

        }




    }
}
