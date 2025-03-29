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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _Db;
        public UnitOfWork(ApplicationDbContext Db) 
        {
            _Db = Db;
            Category = new CategoryRepository(_Db);
            Product=new ProductRepository(_Db);
            Company=new CompanyRepository(_Db);
            ShoppingCart = new ShoppingCartRepository(_Db);
            applicationUser=new ApplicationUserRepository(_Db);
            OrderDetail=new OrderDetailRepository(_Db);
            OrderHeader=new OrderHeaderRepository(_Db);
        }
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product {  get; private set; }

        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart {  get; private set; }

        public IApplicationUser applicationUser { get; private set; }

        public IOrderDetailRepository OrderDetail {  get; private set; }

        public IOrderHeaderRepository OrderHeader {  get; private set; }

        public void Save()
        {
            _Db.SaveChanges();
        }
    }
}
