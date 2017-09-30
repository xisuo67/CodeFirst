using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBServer
{
    /// <summary>
    /// 通用存储库
    /// </summary>
    /// <typeparam name="TEntity">数据实体</typeparam>
    public class GenericRepository<TEntity> : IDisposable where TEntity : class
    {
        internal DBContentSev context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(DBContentSev context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsNoTracking().ToList();
            }
            else
            {
                return query.AsNoTracking().ToList();
            }

        }
        public virtual IEnumerable<TEntity> Query()
        {
            IQueryable<TEntity> query = dbSet;
            return query.AsNoTracking().ToList();
        }
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual bool Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
            //Dispose();
            return true;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);

        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            context.SaveChanges();
            Dispose();
        }

        public virtual bool Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
            Dispose();
            return true;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
