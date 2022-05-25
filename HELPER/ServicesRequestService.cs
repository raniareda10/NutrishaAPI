using AutoMapper;
using BL.Helpers;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.ServiceRequestDTO;
using DL.DTOs.UserDTOs;
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


    public interface IServiceRequestService
    {
        PagedList<ServiceRequestCasher> GetAllServiceRequest(ServiceRequestQueryPramter ServiceRequestParameters);
        public void cahngePersonServiceRequestStatus(int personDataID, int serviceId, int oldRequestStatusID);
    }

    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppPageService _AppPageService; 
        private ILoggerManager _logger;

        public ServiceRequestService(IAppPageService AppPageService,  
            IMapper mapper, 
            IUnitOfWork unitOfWork,
            ILoggerManager logger
            )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _AppPageService = AppPageService;
        }




        public PagedList<ServiceRequestCasher> GetAllServiceRequest(ServiceRequestQueryPramter ServiceRequestParameters)
        {


            List<ServiceRequestCasher> newserviceRequest = new List<ServiceRequestCasher>();

            var appPageSetting = _AppPageService.getPageSetting("ReCaPosX1");

            if (appPageSetting != null)
            {
                


                    var ServiceRequest = _unitOfWork.ServiceRequestRepository.GetMany(dt=> dt.IsActive == true && dt.IsDeleted == false)
                    .Where(dt => dt.RequestStatusId == appPageSetting.RequestStatusId
                     &&dt.ServiceId== appPageSetting.ServiceId )
                      .Include(dt => dt.PersonData)
                      .Include(dt => dt.Service)
                      .Include(dt => dt.PaymentMethod)
                      .Include(dt => dt.DiscountType)
                      .Include(dt => dt.RequestStatus)
                      .Include(dt => dt.PersonData.Gender)
                      .Include(dt => dt.PersonData.Nationality).ToList();



                 
                    foreach (var item in ServiceRequest)
                    {
                        ServiceRequestCasher newserviceRequestitem = new ServiceRequestCasher();
                        var ServiceRequestCasherEntity = _mapper.Map<ServiceRequestCasher>(item);


                        newserviceRequestitem = ServiceRequestCasherEntity;



                        var personJob = _unitOfWork.PersonJobRepository.GetMany(dt => dt.PersonDataId == item.PersonDataId && dt.IsActive == true && dt.IsDeleted == false).Include(dt=>dt.Jobs);
                         if(personJob  != null)
                        {
                            newserviceRequestitem.PersonJob = personJob.OrderBy(dt=>dt.Id).LastOrDefault();
                        }


                        var personStatus = _unitOfWork.PersonStatusRepository.GetMany(dt => dt.PersonDataId == item.PersonDataId && dt.IsActive == true && dt.IsDeleted == false && dt.StatusTypeId==appPageSetting.StatusTypeId).Include(dt=>dt.StatusType);

                        if(personStatus != null)
                        {

                            newserviceRequestitem.PersonStatus = personStatus.OrderBy(dt => dt.Id).LastOrDefault();
                        }

                         

                        newserviceRequest.Add(newserviceRequestitem);


                    }

                    IQueryable<ServiceRequestCasher> PersonData = newserviceRequest.Where(dt=>dt.PersonStatus.Id== appPageSetting.StatusTypeId).AsQueryable();
                    
                    SearchByPramter(ref PersonData, ServiceRequestParameters);

                    return PagedList<ServiceRequestCasher>.ToPagedList(PersonData.AsQueryable(),
                    ServiceRequestParameters.PageNumber,
                    ServiceRequestParameters.PageSize);




                 
                

            }
            return null;
        }



        private void SearchByPramter(ref IQueryable<ServiceRequestCasher> ServiceRequest, ServiceRequestQueryPramter Searchpramter)
        {


            if (ServiceRequest.Any()) { 
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameAr))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.FirstNameAr.ToLower().Contains(Searchpramter.PersonFirstNameAr.Trim().ToLower()));
            }
                if ( Searchpramter.PersonDataID>0)
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonDataId== Searchpramter.PersonDataID);
                }

                if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameAr))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.SecondNameAr.ToLower().Contains(Searchpramter.PersonSecondNameAr.Trim().ToLower()));
            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameAr))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.FamilyNameAr.ToLower().Contains(Searchpramter.PersonFamilyNameAr.Trim().ToLower()));
            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameEN))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.FirstNameEN.ToLower().Contains(Searchpramter.PersonFirstNameEN.Trim().ToLower()));
            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameEN))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.SecondNameEN.ToLower().Contains(Searchpramter.PersonSecondNameEN.Trim().ToLower()));
            }

            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameEN))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.FamilyNameEN.ToLower().Contains(Searchpramter.PersonFamilyNameEN.Trim().ToLower()));


            }

            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersoncivilID))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.civilID.ToLower().Contains(Searchpramter.PersoncivilID.Trim().ToLower()));

            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonpassportNum))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.passportNum.ToLower().Contains(Searchpramter.PersonpassportNum.Trim().ToLower()));

            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonphoneNum))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonData.phoneNum.ToLower().Contains(Searchpramter.PersonphoneNum.Trim().ToLower()));


            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameAr))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonJob.Jobs.NameAr.ToLower().Contains(Searchpramter.PersonJobNameAr.Trim().ToLower()));


            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameEn))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonJob.Jobs.NameEn.ToLower().Contains(Searchpramter.PersonJobNameEn.Trim().ToLower()));



            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameArabic))
                {
                    ServiceRequest = ServiceRequest.Where(o => o.PersonStatus.StatusType.NameArabic.ToLower().Contains(Searchpramter.PersonStatusNameArabic.Trim().ToLower()));
            }
            if (!  string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameEng))
                { 
            ServiceRequest = ServiceRequest.Where(o => o.PersonStatus.StatusType.NameEng.ToLower().Contains(Searchpramter.PersonStatusNameEng.Trim().ToLower()));

                }
            }






        }




        public void cahngePersonServiceRequestStatus(int personDataID,int serviceId,int oldRequestStatusID)
        {
            try
            {
                var ServiceRequestItem = _unitOfWork.ServiceRequestRepository.Get(dt => dt.PersonDataId == personDataID
                  && dt.ServiceId == serviceId && dt.RequestStatusId == oldRequestStatusID && dt.IsActive == true && dt.IsDeleted == false);

                if(ServiceRequestItem !=null)
                {

                    ServiceRequestItem.RequestStatusId = 3;
                    _unitOfWork.ServiceRequestRepository.Update(ServiceRequestItem);
                    _unitOfWork.Save();
                }
                
                
                 

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllServiceRequests action: {ex.Message}");
                //return StatusCode(500, "Internal server error");

            }
        }


    }
}
