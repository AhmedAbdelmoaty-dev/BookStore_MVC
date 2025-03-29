using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _Db;
        public OrderDetailRepository(ApplicationDbContext Db):base(Db) 
        {
            _Db= Db;
        }

        public void Update(OrderDetail orderDetail)
        {
            _Db.Update(orderDetail);
        }
    }
}
