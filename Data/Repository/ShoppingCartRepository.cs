using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _Db;
        public ShoppingCartRepository(ApplicationDbContext db):base(db) 
        {
            _Db=db;
        }
        public void Update(ShoppingCart shoppingCart)
        {
            _Db.Update(shoppingCart);
        }
    

    
    }
}
