using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoCRUD<TEntity> : RepoBase where TEntity : class
    {
        #region Properties

        internal DbSet<TEntity> dbSet;

        #endregion

        #region Constructor

        public RepoCRUD(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext)
        {
            this.dbSet = this.TripickContext.Set<TEntity>();
        }

        #endregion

        public virtual List<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public virtual TEntity GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            this.TripickContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (this.TripickContext.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
        }
    }
}
