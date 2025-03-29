using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _Db;
        public OrderHeaderRepository(ApplicationDbContext Db):base(Db) 
        {
            _Db= Db;
        }

        public void Update(OrderHeader OrderHeader)
        {
            _Db.Update(OrderHeader);
        }

        public void UpdateStauts(int ID, string OrderStatus, string? PaymentStatus = null)
        {
            var OrderFromDb=_Db.OrderHeaders.FirstOrDefault(x => x.ID==ID);
            if (OrderFromDb != null)
            {
                OrderFromDb.OrderStatus = OrderStatus;
                if (!string.IsNullOrEmpty(PaymentStatus))
                {
                    OrderFromDb.PaymentStatus=PaymentStatus;
                } 
            }
        }

        public void UpdateStribePaymentID(int ID, string SessionID, string PaymentIntentID)
        {
            var OrderFromDb = _Db.OrderHeaders.FirstOrDefault(temp => temp.ID == ID);
            if (OrderFromDb != null)
            {
                if (!string.IsNullOrEmpty(SessionID))
                {
                    OrderFromDb.SesssionID = SessionID;
                }
                if (!string.IsNullOrEmpty(PaymentIntentID))
                {
                    OrderFromDb.PaymentIntentID = PaymentIntentID;
                    OrderFromDb.PaymentDate = DateTime.Now;

                }
            }
        }
    }
}
