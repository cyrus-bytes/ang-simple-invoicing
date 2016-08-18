﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesMvcAngular.Domain.Interfaces
{
    public interface IRepository<T>
          where T : class
    {
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        IQueryable<T> GetAll();


        IQueryable<T> Find(Expression<Func<T, bool>> predicate);


        T FindById(int id);
     
        bool Add(T newEntity);

        void Remove(T entity);
        void Update(T entity);


    }
}
