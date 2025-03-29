using DataAccess.Repository.IRepository;
using Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository:IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        void UpdateStauts(int ID, string OrderStatus, string? PaymentStatus = null);
        public void UpdateStribePaymentID(int ID,string SessionID,string PaymentIntentID);

    }
}
