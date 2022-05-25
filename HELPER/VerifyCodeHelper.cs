using BL.Infrastructure;
using DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
  public   class VerifyCodeHelper
    {
        private  readonly IUnitOfWork _uow;
        public readonly ISMS _SMS;
    

        public VerifyCodeHelper( IUnitOfWork uow, ISMS SMS)
        {
            _uow = uow;
            _SMS = SMS;

        }
        public  bool SendOTP(string MobileNum,int userId)
        {
            if (MobileNum!=null  && userId!=0)
            {
                int num = new Random().Next(1000, 9999);
                var VC = new MVerfiyCode { Date = DateTime.Now.AddMinutes(5), Mobile = MobileNum, UserId = userId, VirfeyCode = num };
                _uow.VerfiyCodeRepository.Add(VC);
                _uow.Save();
                try
                {
                    var s = _SMS.SendSMS(MobileNum, num.ToString());
                }
                catch (Exception ex)
                {
                    VC.IsOtpVerfiy = false;
                    _uow.VerfiyCodeRepository.Add(VC);
                    _uow.Save();
                }
                    return true;

                }
            return false;
            
        }
        public bool ActivateOTP(int VerfiyCode)
        {
            var Entity = _uow.VerfiyCodeRepository.GetMany(a => a.VirfeyCode == VerfiyCode).FirstOrDefault();
            if (Entity != null) 
            {
                if (Entity.Date < DateTime.Now)
                {
                    return false;
                }
                var User = _uow.UserRepository.GetById(Entity.UserId);
                User.IsActive = true;
                _uow.UserRepository.Update(User);
                _uow.Save();
                return true;

            }
            return false;
        }
    }
}
