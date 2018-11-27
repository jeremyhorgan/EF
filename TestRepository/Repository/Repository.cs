using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TestRepository.Model;

namespace TestRepository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext Context;

        public Repository(AppDbContext context)
        {
            Context = context;
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            // Set just this one to modified. If entity contains an object graph override 
            // this method and explicitly set the members of the object graph
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }
}