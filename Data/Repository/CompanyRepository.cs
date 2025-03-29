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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _DB;
        public CompanyRepository(ApplicationDbContext db):base(db) 
        {
            _DB=db;
        }
        public void Update(Company company)
        {
            _DB.Update(company);
        }
      
    }
}
