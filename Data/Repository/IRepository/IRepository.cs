using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> Filter, string? includeProperties = null,bool tracking=false);
       
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter=null, string? includeProperties = null);

        void Add(T Record);
        
        void Delete(T Record);

        void DeleteRange(IEnumerable<T> Records);
    }
}
