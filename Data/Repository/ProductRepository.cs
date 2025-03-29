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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _Db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _Db = db;
        }
        //update method separated here as it can contain a manual mapping and therefore
        //logic will differ from a repository to another 
        public void Update(Product product)
        {
            _Db.Products.Update(product);
        }
    }
}
