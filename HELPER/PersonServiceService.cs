using AutoMapper;
using BL.Helpers;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.ServiceDTO;
using DL.DTOs.ServiceRequestDTO;
using DL.DTOs.UserDTOs;
using DL.Entities;
using DL.MailModels;
using HELPER;
using Helpers;
using LoggerService;
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


    public interface IPersonServiceService
    {


        PagedList<Service> GetAllPersonService(ServiceQueryPramter ServiceRequestParameters);
   
    
    
    
    }

    public class PersonServiceService : IPersonServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppPageService _AppPageService; 
        private ILoggerManager _logger;

        public PersonServiceService(IAppPageService AppPageService,  
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            ILoggerManager logger
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _AppPageService = AppPageService;
        }




        public PagedList<Service> GetAllPersonService(ServiceQueryPramter ServiceRequestParameters)
        {

            List<Service>  servicelist= new List<Service>();
           
            if(ServiceRequestParameters.PersonDataId > 0) {


                var personStatusActiveList = _unitOfWork.PersonStatusRepository.GetMany(dt => dt.PersonDataId == ServiceRequestParameters.PersonDataId && dt.IsActive == true && dt.IsDeleted == false).ToList();

                foreach (var item in personStatusActiveList)
                {



                    var avialableServiceFromServiceSetting = _unitOfWork.ServiceSettingRepository.GetMany(q => q.StatusTypeId == item.StatusTypeId && q.isAvailableToChoose == true && q.isAutomatically == false && q.IsActive == true && q.IsDeleted == false).Select(s=>s.ServiceId).ToList();

                    if(avialableServiceFromServiceSetting != null)
                    {
                        var selectedService = _unitOfWork.ServiceRepository.GetMany(dt => avialableServiceFromServiceSetting.Contains(dt.Id)).ToList();
                        foreach (var selectedServiceItem in selectedService)
                        {
                            var testIfServiceExist = servicelist.Where(dt => dt.Id == selectedServiceItem.Id).ToList();
                            if(testIfServiceExist.Count==0)
                            {
                                servicelist.Add(selectedServiceItem);
                            }

                            
                        }

                    }


                }


            }



            return PagedList<Service>.ToPagedList(servicelist.AsQueryable(),
            ServiceRequestParameters.PageNumber,
            ServiceRequestParameters.PageSize);


        }









    }
}
