using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _Db;
        internal DbSet<T> dbSet; 
        public Repository(ApplicationDbContext Db)
        {
            _Db = Db;
            dbSet=_Db.Set<T>();
            
        }
        public void Add(T Record)
        {
            dbSet.Add(Record);
        }
        public T Get(Expression<Func<T, bool>> Filter, string? includeProperties = null, bool tracking = false )
        {
            IQueryable<T> query;
            if (tracking)
            {
                query = dbSet;
            }
            else
            {
                query= dbSet.AsNoTracking();
            }
            query= query.Where(Filter);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                   query= query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter=null,string? includeProperties=null)
        {
            IQueryable<T> query=dbSet;
            if (Filter != null)
            {
               query = dbSet.Where(Filter);
            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char [] { ','},StringSplitOptions.RemoveEmptyEntries ))
                {
                    query=query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Delete(T Record)
        {
            dbSet.Remove(Record);
        }

        public void DeleteRange(IEnumerable<T> Records)
        {
            dbSet.RemoveRange(Records);
        }


    }
    
}
