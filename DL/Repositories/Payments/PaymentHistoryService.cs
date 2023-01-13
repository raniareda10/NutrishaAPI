using System.Threading.Tasks;
using DL.DBContext;
using DL.EntitiesV1.Payments;

namespace DL.Repositories.Payments
{
    public class PaymentHistoryService
    {
        private readonly AppDBContext _appDbContext;

        public PaymentHistoryService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public async Task AddAsync(PaymentHistoryEntity paymentHistoryEntity)
        {
            await _appDbContext.PaymentHistory.AddAsync(paymentHistoryEntity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}