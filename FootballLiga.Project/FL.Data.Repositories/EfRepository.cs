using FL.Common.Base.Data;
using FL.Data.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FL.Data.Repositories
{
    /// <summary>
    /// Represents the Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly IDbContext _context;

        private DbSet<TEntity> _entities;

        #endregion

        #region Ctor

        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                //if after the rollback of changes the context is still not saving,
                //return the full text of the exception that occurred when saving
                return ex.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities"></param>
        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.RemoveRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public void DeleteAll(List<Guid> ids)
        {
            foreach (var item in ids)
            {
                TEntity entityToDelete = Entities.Find(item);
                Delete(entityToDelete);
            }
            _context.SaveChanges();
        }

        public void DeleteAllByIds<TId>(List<TId> ids)
        {
            foreach (var item in ids)
            {
                TEntity entityToDelete = Entities.Find(item);
                Delete(entityToDelete);
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public TEntity Get(object id, string includeProperties)
        {
            if (includeProperties != null)
            {
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Entities.Include(includeProperty);
                }
            }
            return Entities.Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties)
        {
            try
            {
                if (includeProperties != null)
                {
                    foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Entities.Include(includeProperty);
                    }
                }
                return Entities.Single(filter);
            }
            catch (Exception e)
            {
                var message = string.Format(
                    "{0} (Collection: {1}, param: {2})",
                    e.Message,
                    filter.GetType(),
                    filter);
                throw new Exception("Database connection: SingleOrDefault failed");
            }
        }

        /// <summary>
        /// Gets the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return Entities.SingleOrDefault(filter);
            }
            catch (Exception e)
            {
                var message = string.Format(
                    "{0} (Collection: {1}, param: {2})",
                    e.Message,
                    filter.GetType(),
                    filter);
                throw new Exception("Database connection: SingleOrDefault failed");
            }
        }

        /// <summary>
        /// Gets the first or default.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return Entities.FirstOrDefault(filter);
        }

        /// <summary>
        /// Gets the first or default.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string includeProperties)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Entities.Include(includeProperty);
                }
            }
            return Entities.FirstOrDefault(filter);
        }

        public IEnumerable<TEntity> GetList(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes.Any())
                return Includes(includes, query);

            return query;
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> whereCondition, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(whereCondition);

            if (includes.Any())
                return Includes(includes, query);

            return query;
        }

        public TEntity FindBy(Expression<Func<TEntity, bool>> whereCondition, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>().Where(whereCondition);

            if (includes.Any())
                return Includes(includes, query).FirstOrDefault();

            return query.FirstOrDefault();
        }

        protected IQueryable<TEntity> Includes(Expression<Func<TEntity, object>>[] includes, IQueryable<TEntity> query)
        {
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.AddRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                Entities.UpdateRange(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();

                return _entities;
            }
        }

        #endregion
    }
}
