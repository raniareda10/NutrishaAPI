using System;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Firebase.Database;
using BL.Infrastructure;
using System.Linq;

namespace NutrishaAPIAPI
{
    public class ClearFireBaseJob : IClearFireBaseJob
    {
        private readonly IUnitOfWork _uow;
        public  ClearFireBaseJob (IUnitOfWork uow)
        {
            _uow = uow;
          
        }
        public async Task ClearOfferAsync()
        {
            var date = DateTime.UtcNow;
            var LstUserId = _uow.UserRepository.GetAll().Where(c => c.OfferId != null && c.OfferTime != null && c.OfferTime.Value.AddMinutes(1) < date).Select(c => c.Id);
            foreach (var userId in LstUserId)
            {

                var firebase = new FirebaseClient("https://aldamina-logistics-default-rtdb.firebaseio.com/");
                await firebase
                    .Child("User")
                    .Child(userId.ToString())
                    .Child("offer")
                    .DeleteAsync();
                var user = _uow.UserRepository.GetById(userId);
                user.OfferId = null;
                user.OfferTime = null;
                _uow.UserRepository.Update(user);
                // _uow.Save();
            }
        }

        void IClearFireBaseJob.ClearOfferAsync()
        {
        
        }
    }
}
