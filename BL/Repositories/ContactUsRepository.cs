using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.ContactUsDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BL.Repositories
{
    public interface IContactUsRepository
    {
        PagedList<MContactUs> GetAllContactUs(ContactUsQueryPramter ContactUsParameters);

    }

    public class ContactUsRepository : Repository<MContactUs>, IContactUsRepository
    {
        public ContactUsRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MContactUs> GetAllContactUs(ContactUsQueryPramter ContactUsParameters)
        {

             

            IQueryable<MContactUs> ContactUs = GetAll();


            //searhing
            SearchByPramter(ref ContactUs, ContactUsParameters.Name);

            return PagedList<MContactUs>.ToPagedList(ContactUs,
                ContactUsParameters.PageNumber,
                ContactUsParameters.PageSize);




        }

        private void SearchByPramter(ref IQueryable<MContactUs> ContactUs, string Searchpramter)
        {
            if (!ContactUs.Any() || string.IsNullOrWhiteSpace(Searchpramter))
                return;
            ContactUs = ContactUs.Where(o => o.Name.ToLower().Contains(Searchpramter.Trim().ToLower()));
        }


    }
}
