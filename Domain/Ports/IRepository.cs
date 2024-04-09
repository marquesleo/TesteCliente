using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public  interface IRepository<T>
    {
        public Task<T> GetById(int id);

        public Task<T> Save(T entity);

        public Task DeleteById(int id); 

        public Task<IEnumerable<T>> GetAll();   
    }
}
