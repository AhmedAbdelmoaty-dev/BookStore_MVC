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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _Db;
        public CategoryRepository(ApplicationDbContext Db):base(Db) 
        {
            _Db= Db;
        }

        public void Update(Category category)
        {
            _Db.Update(category);
        }
    }
}
