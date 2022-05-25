using AutoMapper;
using BL.Helpers;
using BL.Infrastructure;
using BL.Security;
using DL.DTO;
using DL.DTOs.CasherDTO;
using DL.DTOs.PersonStatusDTO;
using DL.DTOs.ServiceRequestDTO;
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


    public interface IPersonService
    {
        PagedList<PersonModel> GetAllPersonDatapage(PersonDataPageQueryPramter PersonDataPageQueryPramter);
       
        
        public PagedList<PersonModel> GetAllPersonStatus(PersonStatusQueryPramter PersonDataPageQueryPramter);


    }

    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IAppPageService _AppPageService;

        public PersonService(IAppPageService AppPageService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _AppPageService = AppPageService;
        }


        public PagedList<PersonModel> GetAllPersonStatus(PersonStatusQueryPramter PersonDataPageQueryPramter)
        {
             
                List<PersonModel> newserviceRequest = new List<PersonModel>();
             
                var personDataList = _unitOfWork.PersonDataRepository.GetMany(dt => dt.IsActive == true && dt.IsDeleted == false)
                             // .Include(dt=>dt.PersonStatus).Include(dt => dt.PersonJob).Include(dt => dt.Vaccine)
                             .Include(dt => dt.Gender)
                             .Include(dt => dt.Nationality).ToList().AsQueryable();

                foreach (var item in personDataList)
                {
                    PersonModel PersonModelItem = new PersonModel();

                    var PersonModelItemEntity = _mapper.Map<PersonModel>(item);


                    PersonModelItem = PersonModelItemEntity;


 

                    var personStatus = _unitOfWork.PersonStatusRepository.GetMany(dt => dt.PersonDataId == item.Id  ).Include(dt => dt.StatusType);

                    if (personStatus != null)
                    {

                        PersonModelItem.PersonStatus = personStatus.OrderBy(dt => dt.Id).ToList();
                    }


                     

                     
                     

                   
                       
                            newserviceRequest.Add(PersonModelItem);
                         
                     

                }

                IQueryable<PersonModel> PersonData = newserviceRequest.AsQueryable();
            ////searhing
            SearchByPersonDataPramter(ref PersonData, PersonDataPageQueryPramter);

                return PagedList<PersonModel>.ToPagedList(PersonData.AsQueryable(),
                PersonDataPageQueryPramter.PageNumber,
                PersonDataPageQueryPramter.PageSize);




 

          
        }


        public PagedList<PersonModel> GetAllPersonDatapage(PersonDataPageQueryPramter PersonDataPageQueryPramter)
        {


          
            var appPageSetting = _AppPageService.getPageSetting(PersonDataPageQueryPramter.pageCode);

            if (appPageSetting != null)
            {



                List<PersonModel> newserviceRequest = new List<PersonModel>();

                 

                var personDataList = _unitOfWork.PersonDataRepository.GetMany(dt=>dt.IsActive==true&&dt.IsDeleted==false)
                             // .Include(dt=>dt.PersonStatus).Include(dt => dt.PersonJob).Include(dt => dt.Vaccine)
                             .Include(dt => dt.Gender)
                             .Include(dt => dt.Nationality).ToList().AsQueryable();

                foreach (var item in personDataList)
                {
                    PersonModel PersonModelItem = new PersonModel();

                    var PersonModelItemEntity = _mapper.Map<PersonModel>(item);


                    PersonModelItem = PersonModelItemEntity;



                    var personJob = _unitOfWork.PersonJobRepository.GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false).Include(dt => dt.Jobs);
                    if (personJob != null)
                    {
                        PersonModelItem.PersonJob = personJob.OrderBy(dt => dt.Id).ToList();
                    }


                    var personAppoinment = _unitOfWork.AppointmentRepository.GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false);
                    if (personAppoinment != null)
                    {
                        PersonModelItem.personAppoinment = personAppoinment.OrderBy(dt => dt.Id).ToList();
                    }


                    var personCertificate = _unitOfWork.CertificateRepository.GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false).Include(dt=>dt.CertType);
                    if (personCertificate != null)
                    {
                        PersonModelItem.Certificate = personCertificate.OrderBy(dt => dt.Id).ToList();
                    }


                    var personStatus = _unitOfWork.PersonStatusRepository.GetMany(dt => dt.PersonDataId == item.Id&&dt.IsActive == true && dt.IsDeleted == false && dt.StatusTypeId == appPageSetting.StatusTypeId).Include(dt => dt.StatusType);

                    if (personStatus != null)
                    {

                        PersonModelItem.PersonStatus = personStatus.OrderBy(dt => dt.Id).ToList();
                    }



                    var personVaccine = _unitOfWork.VaccineRepository.GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false).Include(dt => dt.VaccineDose).Include(dt => dt.VaccineType);
                    if (personVaccine != null)
                    {
                        PersonModelItem.Vaccine = personVaccine.OrderBy(dt => dt.Id).ToList();
                    }


                    var personServiceRequest = _unitOfWork.ServiceRequestRepository
                        .GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false && dt.RequestStatusId == appPageSetting.RequestStatusId && dt.ServiceId == appPageSetting.ServiceId).Include(dt=>dt.Service);
                    if (personServiceRequest != null)
                    {
                        PersonModelItem.ServiceRequest = personServiceRequest.OrderBy(dt => dt.Id).ToList();
                    }


                    var personCasher = _unitOfWork.CasherRepository
                        .GetMany(dt => dt.PersonDataId == item.Id && dt.IsActive == true && dt.IsDeleted == false && dt.ServiceId == appPageSetting.ServiceId && dt.ServiceRequest.RequestStatusId == appPageSetting.RequestStatusId).Include(dt=>dt.ServiceRequest);
                    if (personCasher != null)
                    {
                        PersonModelItem.Casher = personCasher.OrderBy(dt => dt.Id).ToList();
                    }


                    if (appPageSetting.ApplicationPageId==1 ) { 

                        if (PersonModelItem.PersonStatus.Where(dt =>
                         dt.StatusTypeId == appPageSetting.StatusTypeId).Any()
                         && PersonModelItem.ServiceRequest.Where(dt => dt.RequestStatusId == appPageSetting.RequestStatusId && dt.ServiceId == appPageSetting.ServiceId).Any()
                        )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }
                    }

                    else if (appPageSetting.ApplicationPageId == 2)
                    {

                        if (PersonModelItem.PersonStatus.Where(dt =>
                         dt.StatusTypeId == appPageSetting.StatusTypeId).Any()
                         && PersonModelItem.Casher.Where(dt => dt.ServiceId == appPageSetting.ServiceId && dt.ServiceRequest.RequestStatusId == appPageSetting.RequestStatusId).Any()
                        )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }
                    }

                    else if (appPageSetting.ApplicationPageId == 3)
                    {
                        if (PersonModelItem.PersonStatus.Where(dt =>
                       dt.StatusTypeId == appPageSetting.StatusTypeId).Any()
                       
                       )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }

                    }

                    else if (appPageSetting.ApplicationPageId == 4)
                    {
                        if (PersonModelItem.PersonStatus.Where(dt =>
                       dt.StatusTypeId == appPageSetting.StatusTypeId).Any()
                      
                       )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }

                    }

                    if (appPageSetting.ApplicationPageId == 5)
                    {

                        if ( PersonModelItem.ServiceRequest.Where(dt => dt.RequestStatusId == appPageSetting.RequestStatusId && dt.ServiceId == appPageSetting.ServiceId).Any()
                        )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }
                    }

                    else if (appPageSetting.ApplicationPageId == 6)
                    {

                        if (
                         PersonModelItem.ServiceRequest.Where(dt => dt.RequestStatusId == appPageSetting.RequestStatusId && dt.ServiceId == appPageSetting.ServiceId).Any()
                        )

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }
                    }


                    else if (appPageSetting.ApplicationPageId == 13)
                    {

                        if (PersonModelItem.PersonStatus.Where(dt =>
                       dt.StatusTypeId == appPageSetting.StatusTypeId).Any())

                        {
                            newserviceRequest.Add(PersonModelItem);
                        }
                    }

                }

                IQueryable<PersonModel> PersonData = newserviceRequest.AsQueryable();
                ////searhing
                SearchByPramter(ref PersonData, PersonDataPageQueryPramter);

                return PagedList<PersonModel>.ToPagedList(PersonData.AsQueryable(),
                PersonDataPageQueryPramter.PageNumber,
                PersonDataPageQueryPramter.PageSize);





            }

            return null;
        }


        private void SearchByPramter(ref IQueryable<PersonModel> PersonDataModel, PersonDataPageQueryPramter Searchpramter)
        {


            if (PersonDataModel.Any())
            {
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FirstNameAr.ToLower().Contains(Searchpramter.PersonFirstNameAr.Trim().ToLower()));
                }
                if (Searchpramter.PersonDataID > 0)
                {
                    PersonDataModel = PersonDataModel.Where(o => o.Id == Searchpramter.PersonDataID);
                }

                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.SecondNameAr.ToLower().Contains(Searchpramter.PersonSecondNameAr.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FamilyNameAr.ToLower().Contains(Searchpramter.PersonFamilyNameAr.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FirstNameEN.ToLower().Contains(Searchpramter.PersonFirstNameEN.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.SecondNameEN.ToLower().Contains(Searchpramter.PersonSecondNameEN.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FamilyNameEN.ToLower().Contains(Searchpramter.PersonFamilyNameEN.Trim().ToLower()));


                }

                if (!string.IsNullOrWhiteSpace(Searchpramter.PersoncivilID))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.civilID.ToLower().Contains(Searchpramter.PersoncivilID.Trim().ToLower()));

                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonpassportNum))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.passportNum.ToLower().Contains(Searchpramter.PersonpassportNum.Trim().ToLower()));

                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonphoneNum))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.phoneNum.ToLower().Contains(Searchpramter.PersonphoneNum.Trim().ToLower()));


                }
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameAr))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonJob.Jobs.NameAr.ToLower().Contains(Searchpramter.PersonJobNameAr.Trim().ToLower()));


                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameEn))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonJob.Jobs.NameEn.ToLower().Contains(Searchpramter.PersonJobNameEn.Trim().ToLower()));



                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameArabic))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonStatus.StatusType.NameArabic.ToLower().Contains(Searchpramter.PersonStatusNameArabic.Trim().ToLower()));
                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameEng))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonStatus.StatusType.NameEng.ToLower().Contains(Searchpramter.PersonStatusNameEng.Trim().ToLower()));

                //}
            }





        }

        private void SearchByPersonDataPramter(ref IQueryable<PersonModel> PersonDataModel, PersonStatusQueryPramter Searchpramter)
        {


            if (PersonDataModel.Any())
            {
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FirstNameAr.ToLower().Contains(Searchpramter.PersonFirstNameAr.Trim().ToLower()));
                }
                if (Searchpramter.PersonDataID > 0)
                {
                    PersonDataModel = PersonDataModel.Where(o => o.Id == Searchpramter.PersonDataID);
                }
                if (Searchpramter.UserId > 0)
                {
                    PersonDataModel = PersonDataModel.Where(o => o.UserId == Searchpramter.UserId);
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.SecondNameAr.ToLower().Contains(Searchpramter.PersonSecondNameAr.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameAr))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FamilyNameAr.ToLower().Contains(Searchpramter.PersonFamilyNameAr.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFirstNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FirstNameEN.ToLower().Contains(Searchpramter.PersonFirstNameEN.Trim().ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonSecondNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.SecondNameEN.ToLower().Contains(Searchpramter.PersonSecondNameEN.Trim().ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonFamilyNameEN))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.FamilyNameEN.ToLower().Contains(Searchpramter.PersonFamilyNameEN.Trim().ToLower()));


                }

                if (!string.IsNullOrWhiteSpace(Searchpramter.PersoncivilID))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.civilID.ToLower().Contains(Searchpramter.PersoncivilID.Trim().ToLower()));

                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonpassportNum))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.passportNum.ToLower().Contains(Searchpramter.PersonpassportNum.Trim().ToLower()));

                }
                if (!string.IsNullOrWhiteSpace(Searchpramter.PersonphoneNum))
                {
                    PersonDataModel = PersonDataModel.Where(o => o.phoneNum.ToLower().Contains(Searchpramter.PersonphoneNum.Trim().ToLower()));


                }
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameAr))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonJob.Jobs.NameAr.ToLower().Contains(Searchpramter.PersonJobNameAr.Trim().ToLower()));


                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonJobNameEn))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonJob.Jobs.NameEn.ToLower().Contains(Searchpramter.PersonJobNameEn.Trim().ToLower()));



                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameArabic))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonStatus.StatusType.NameArabic.ToLower().Contains(Searchpramter.PersonStatusNameArabic.Trim().ToLower()));
                //}
                //if (!string.IsNullOrWhiteSpace(Searchpramter.PersonStatusNameEng))
                //{
                //    PersonDataModel = PersonDataModel.Where(o => o.PersonStatus.StatusType.NameEng.ToLower().Contains(Searchpramter.PersonStatusNameEng.Trim().ToLower()));

                //}
            }





        }


    }
}
