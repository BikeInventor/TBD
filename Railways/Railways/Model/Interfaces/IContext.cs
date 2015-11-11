using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Railways.Model.Interfaces
{
    public interface IContext<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
    }
}
