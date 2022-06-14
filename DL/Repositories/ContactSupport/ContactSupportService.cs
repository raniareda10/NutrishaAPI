using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Common;
using DL.DtosV1.ContactSupport;
using DL.EntitiesV1.ContactSupport;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.ContactSupport
{
    public class ContactSupportService
    {
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;

        public ContactSupportService(AppDBContext appDbContext, ICurrentUserService currentUserService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
        }


        public async Task<PayloadServiceResult<long>> PostAsync(CreateContactSupportDto model)
        {
            var contactEntity = new ContactSupportEntity()
            {
                Created = DateTime.UtcNow,
                UserId = _currentUserService.UserId,
                Message = model.Message,
                TypeId = model.TypeId
            };
            
            await _appDbContext.AddAsync(contactEntity);
            await _appDbContext.SaveChangesAsync();
            
            
            return new PayloadServiceResult<long>(contactEntity.Id);
        }


        public async Task<IEnumerable<LookupItem>> GetAllTypes()
        {
            return await _appDbContext.ContactSupportTypes
                .Select(s => new LookupItem(s.Id, s.Name))
                .ToListAsync();
        }
    }
}