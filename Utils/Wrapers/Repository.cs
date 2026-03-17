using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Utils.Wrapers
{
    public class Repository<T> :IRepository<T> where T : class
    {
        protected DbContext _context;
        protected DbSet<T> _set;
        protected Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public void Create(T entity)=> _set.Add(entity);
      
        public void Delete(T entity)=> _set.Remove(entity);


        public T ReadById(object id) => _set.Find(id);
        

        public IEnumerable<T> ReadMany(Expression<Func<T, bool>> expression = null, params string[] includes)
        {
            var data = expression == null ? _set : _set.Where(expression);
            foreach (var include in includes)
            {
                data =data.Include(include);
            }
            return data;
        }

        public void Update(T entity) => _context.Entry(entity).State=EntityState.Modified;
        

       
    }
}
